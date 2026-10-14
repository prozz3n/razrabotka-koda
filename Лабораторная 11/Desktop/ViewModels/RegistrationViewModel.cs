using System;
using System.Threading.Tasks;
using System.Windows;
using Desktop.Navigation;
using Desktop.Repository;
using Desktop.Services;
using Desktop.Validation;
using Todo.Entities;

namespace Desktop.ViewModels
{
    public class RegistrationViewModel : ObservableObject
    {
        private readonly INavigationService _navigation;
        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;

        public RegistrationViewModel(INavigationService navigation)
        {
            _navigation = navigation;
            RegisterCommand = new AsyncRelayCommand(RegisterAsync);
            GoToLoginCommand = new RelayCommand(() => _navigation.Navigate("login"));
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public AsyncRelayCommand RegisterCommand { get; }

        public RelayCommand GoToLoginCommand { get; }

        private async Task RegisterAsync()
        {
            string name = Name.Trim();
            string email = Email.Trim();

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

            if (!InputValidator.IsValidPassword(Password))
            {
                MessageBox.Show("Пароль должен содержать не менее 6 символов.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                AuthResult result = await ApiService.RegisterAsync(name, email, Password);
                if (!result.Success)
                {
                    MessageBox.Show(result.Error, "Ошибка регистрации",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception)
            {
                var localUser = new UserModel
                {
                    Name = name,
                    Login = email,
                    Password = Password
                };

                if (!UserRepository.Register(localUser, out string error))
                {
                    MessageBox.Show(error, "Ошибка регистрации",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            MessageBox.Show("Регистрация прошла успешно. Войдите в аккаунт.",
                "Готово", MessageBoxButton.OK, MessageBoxImage.Information);

            _navigation.Navigate("login");
        }
    }
}
