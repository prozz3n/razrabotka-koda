using System.Windows;
using Desktop.Repository;
using Desktop.Validation;
using Todo.Entities;

namespace Desktop
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
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

            UserModel? user = UserRepository.Authorize(email, password);
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TaskRepository.GetForUser(user.Login).Count == 0)
            {
                new MainEmptyWindow().Show();
            }
            else
            {
                new MainWindow().Show();
            }

            Close();
        }

        private void GoToRegistration_Click(object sender, RoutedEventArgs e)
        {
            new RegistrationWindow().Show();
            Close();
        }
    }
}
