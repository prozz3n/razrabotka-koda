using System;
using System.Threading.Tasks;
using System.Windows;
using Desktop.Navigation;
using Desktop.Repository;
using Desktop.Services;
using Todo.Entities;

namespace Desktop.ViewModels
{
    public class AddTaskViewModel : ObservableObject
    {
        private readonly INavigationService _navigation;
        private readonly string _login;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _category = "Учёба";
        private DateTime? _date = DateTime.Today;

        public AddTaskViewModel(INavigationService navigation)
        {
            _navigation = navigation;
            _login = UserRepository.CurrentUser?.Login ?? string.Empty;

            SaveCommand = new AsyncRelayCommand(SaveAsync);
            CancelCommand = new RelayCommand(Cancel);
        }

        public string[] Categories { get; } = { "Учёба", "Работа", "Личное", "Покупки" };

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        public DateTime? Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public AsyncRelayCommand SaveCommand { get; }

        public RelayCommand CancelCommand { get; }

        private async Task SaveAsync()
        {
            string title = Title.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Введите название задачи.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var task = new TaskModel
            {
                Title = title,
                Description = Description.Trim(),
                Category = string.IsNullOrWhiteSpace(Category) ? "Личное" : Category,
                Date = Date ?? DateTime.Today
            };

            try
            {
                TaskModel? created = await ApiService.CreateTaskAsync(task);
                if (created != null)
                {
                    task = created;
                }
            }
            catch (Exception)
            {
                // Нет связи — сохраняем локально.
            }

            TaskRepository.Add(_login, task);
            _navigation.Navigate("main");
        }

        private void Cancel()
        {
            if (_navigation.CanGoBack)
            {
                _navigation.GoBack();
            }
            else
            {
                _navigation.Navigate("main");
            }
        }
    }
}
