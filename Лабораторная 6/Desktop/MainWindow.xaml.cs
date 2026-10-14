using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Desktop.Repository;
using Todo.Entities;

namespace Desktop
{
    public partial class MainWindow : Window, INotifyPropertyChanged
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

        public MainWindow()
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

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask != null)
            {
                SelectedTask.IsDone = true;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask == null)
            {
                return;
            }

            TaskRepository.Remove(_login, SelectedTask);
            SelectedTask = null;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddTaskWindow { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                TasksView?.Refresh();
            }
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            new HistoryWindow { Owner = this }.ShowDialog();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            UserRepository.Logout();
            new LoginWindow().Show();
            Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
