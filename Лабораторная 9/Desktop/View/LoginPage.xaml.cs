using System;
using System.Windows;
using System.Windows.Controls;
using Desktop.Repository;
using Desktop.Services;
using Desktop.Validation;
using Todo.Entities;

namespace Desktop.View
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password;

            if (!InputValidator.IsValidEmail(email))
            {
                MessageBox.Show("Введите корректный email (например, name@mail.com).",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!InputValidator.IsValidPassword(password))
            {
                MessageBox.Show("Пароль должен содержать не менее 6 символов.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                AuthResult result = await ApiService.LoginAsync(email, password);
                if (!result.Success || result.User == null)
                {
                    MessageBox.Show($"Не удалось войти: {result.Error}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                UserModel user = result.User;
                UserRepository.SetCurrentUser(user);

                var tasks = await ApiService.GetTasksAsync();
                TaskRepository.Replace(user.Login, tasks);

                GoToMain(user);
            }
            catch (Exception)
            {
                // Нет связи с сервером — работаем с локальными данными.
                UserModel? user = UserRepository.Authorize(email, password);
                if (user == null)
                {
                    MessageBox.Show("Нет связи с сервером, а локального пользователя с такими данными нет.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                GoToMain(user);
            }
        }

        private void GoToMain(UserModel user)
        {
            if (TaskRepository.GetForUser(user.Login).Count == 0)
            {
                NavigationService.Navigate(new MainEmptyPage());
            }
            else
            {
                NavigationService.Navigate(new MainPage());
            }
        }

        private void GoToRegistration_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}
