using System;
using System.Windows;
using Desktop.Repository;
using Todo.Entities;

namespace Desktop
{
    public partial class AddTaskWindow : Window
    {
        private readonly string _login;

        public AddTaskWindow()
        {
            InitializeComponent();

            _login = UserRepository.CurrentUser?.Login ?? string.Empty;

            CategoryCombo.ItemsSource = new[] { "Учёба", "Работа", "Личное", "Покупки" };
            CategoryCombo.SelectedIndex = 0;
            DatePicker.SelectedDate = DateTime.Today;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
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

            TaskRepository.Add(_login, task);

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
