using System.Windows;
using System.Windows.Controls;
using Desktop.Repository;
using Desktop.Validation;
using Todo.Entities;

namespace Desktop.View
{
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text.Trim();
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirm = ConfirmPasswordBox.Password;

            if (!InputValidator.IsValidName(name))
            {
                MessageBox.Show("Имя должно содержать не менее 3 символов.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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

            if (password != confirm)
            {
                MessageBox.Show("Пароли не совпадают.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = new UserModel
            {
                Name = name,
                Login = email,
                Password = password
            };

            if (!UserRepository.Register(user, out string error))
            {
                MessageBox.Show(error, "Ошибка регистрации",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Регистрация прошла успешно. Войдите в аккаунт.",
                "Готово", MessageBoxButton.OK, MessageBoxImage.Information);

            NavigationService.Navigate(new LoginPage());
        }

        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}
