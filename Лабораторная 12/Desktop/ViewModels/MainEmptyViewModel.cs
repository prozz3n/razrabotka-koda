using Desktop.Navigation;
using Desktop.Repository;
using Desktop.Services;

namespace Desktop.ViewModels
{
    public class MainEmptyViewModel : ObservableObject
    {
        private readonly INavigationService _navigation;

        public MainEmptyViewModel(INavigationService navigation)
        {
            _navigation = navigation;
            AddTaskCommand = new RelayCommand(() => _navigation.Navigate("addtask"));
            LogoutCommand = new RelayCommand(Logout);
        }

        public string UserName => UserRepository.CurrentUser?.Name ?? "Пользователь";

        public RelayCommand AddTaskCommand { get; }

        public RelayCommand LogoutCommand { get; }

        private void Logout()
        {
            AuthDatabase.Clear();
            ApiService.Reset();
            UserRepository.Logout();
            _navigation.Navigate("login");
        }
    }
}
