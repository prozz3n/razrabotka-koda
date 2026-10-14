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
    public class LoginViewModel : ObservableObject
    {
        private readonly INavigationService _navigation;
        private string _email = string.Empty;
        private string _password = string.Empty;

        public LoginViewModel(INavigationService navigation)
        {
            _navigation = navigation;
            LoginCommand = new AsyncRelayCommand(LoginAsync);
            RegistrationCommand = new RelayCommand(() => _navigation.Navigate("registration"));
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

        public AsyncRelayCommand LoginCommand { get; }

        public RelayCommand RegistrationCommand { get; }

        private async Task LoginAsync()
        {
            string email = Email.Trim();

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

            try
            {
                AuthResult result = await ApiService.LoginAsync(email, Password);
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

                AuthDatabase.Save(ApiService.Token ?? string.Empty, user.Login, user.Name);

                GoToMain(user);
            }
            catch (Exception)
            {
                UserModel? user = UserRepository.Authorize(email, Password);
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
            _navigation.Navigate(
                TaskRepository.GetForUser(user.Login).Count == 0 ? "mainempty" : "main");
        }
    }
}
