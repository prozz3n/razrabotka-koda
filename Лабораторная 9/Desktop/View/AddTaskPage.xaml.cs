using System;
using System.Windows;
using System.Windows.Controls;
using Desktop.Repository;
using Desktop.Services;
using Todo.Entities;

namespace Desktop.View
{
    public partial class AddTaskPage : Page
    {
        private readonly string _login;

        public AddTaskPage()
        {
            InitializeComponent();

            _login = UserRepository.CurrentUser?.Login ?? string.Empty;

            CategoryCombo.ItemsSource = new[] { "Учёба", "Работа", "Личное", "Покупки" };
            CategoryCombo.SelectedIndex = 0;
            DatePicker.SelectedDate = DateTime.Today;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Введите название задачи.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var task = new TaskModel
            {
                Title = title,
                Description = DescriptionBox.Text.Trim(),
                Category = CategoryCombo.SelectedItem?.ToString() ?? "Личное",
                Date = DatePicker.SelectedDate ?? DateTime.Today
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

            NavigationService.Navigate(new MainPage());
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                NavigationService.Navigate(new MainPage());
            }
        }
    }
}
