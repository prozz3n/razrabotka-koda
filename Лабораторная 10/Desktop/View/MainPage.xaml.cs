using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Desktop.Repository;
using Desktop.Services;
using Todo.Entities;

namespace Desktop.View
{
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        private readonly string _login;
        private string _selectedCategory = "Все";
        private TaskModel? _selectedTask;

        public string UserName { get; }

        public ObservableCollection<string> Categories { get; }

        public ObservableCollection<TaskModel> Tasks { get; }

        public ICollectionView TasksView { get; }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                TasksView?.Refresh();
            }
        }

        public TaskModel? SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            InitializeComponent();

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

            DataContext = this;
        }

        private bool FilterTask(object obj)
        {
            if (obj is not TaskModel task)
            {
                return false;
            }

            return _selectedCategory == "Все" || task.Category == _selectedCategory;
        }

        private async void Done_Click(object sender, RoutedEventArgs e)
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

        private async void Delete_Click(object sender, RoutedEventArgs e)
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

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddTaskPage());
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HistoryPage());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            AuthDatabase.Clear();
            ApiService.Reset();
            UserRepository.Logout();
            NavigationService.Navigate(new LoginPage());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
