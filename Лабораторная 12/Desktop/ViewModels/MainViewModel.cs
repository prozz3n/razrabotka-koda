using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using Desktop.Navigation;
using Desktop.Repository;
using Desktop.Services;
using Todo.Entities;

namespace Desktop.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly INavigationService _navigation;
        private readonly string _login;
        private string _selectedCategory = "Все";
        private TaskModel? _selectedTask;

        public MainViewModel(INavigationService navigation)
        {
            _navigation = navigation;

            UserModel? user = UserRepository.CurrentUser;
            _login = user?.Login ?? string.Empty;
            UserName = user?.Name ?? "Пользователь";

            Categories = new ObservableCollection<string>
            {
                "Все", "Учёба", "Работа", "Личное", "Покупки"
            };

            Tasks = TaskRepository.GetForUser(_login);
            TasksView = CollectionViewSource.GetDefaultView(Tasks);
            TasksView.Filter = FilterTask;

            DoneCommand = new AsyncRelayCommand(DoneAsync);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync);
            AddTaskCommand = new RelayCommand(() => _navigation.Navigate("addtask"));
            HistoryCommand = new RelayCommand(() => _navigation.Navigate("history"));
            LogoutCommand = new RelayCommand(Logout);
        }

        public string UserName { get; }

        public ObservableCollection<string> Categories { get; }

        public ObservableCollection<TaskModel> Tasks { get; }

        public ICollectionView TasksView { get; }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                {
                    TasksView.Refresh();
                }
            }
        }

        public TaskModel? SelectedTask
        {
            get => _selectedTask;
            set => SetProperty(ref _selectedTask, value);
        }

        public AsyncRelayCommand DoneCommand { get; }

        public AsyncRelayCommand DeleteCommand { get; }

        public RelayCommand AddTaskCommand { get; }

        public RelayCommand HistoryCommand { get; }

        public RelayCommand LogoutCommand { get; }

        private bool FilterTask(object obj)
        {
            if (obj is not TaskModel task)
            {
                return false;
            }

            return _selectedCategory == "Все" || task.Category == _selectedCategory;
        }

        private async Task DoneAsync()
        {
            if (SelectedTask == null)
            {
                return;
            }

            SelectedTask.IsDone = true;

            try
            {
                await ApiService.UpdateTaskAsync(SelectedTask);
            }
            catch (Exception)
            {
                // Нет связи — статус сохранён локально.
            }
        }

        private async Task DeleteAsync()
        {
            if (SelectedTask == null)
            {
                return;
            }

            TaskModel task = SelectedTask;
            TaskRepository.Remove(_login, task);
            SelectedTask = null;

            try
            {
                await ApiService.DeleteTaskAsync(task.Id);
            }
            catch (Exception)
            {
                // Нет связи — удалено локально.
            }
        }

        private void Logout()
        {
            AuthDatabase.Clear();
            ApiService.Reset();
            UserRepository.Logout();
            _navigation.Navigate("login");
        }
    }
}
