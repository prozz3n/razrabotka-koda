using System.Windows;
using System.Windows.Controls;
using Desktop.Repository;
using Desktop.Services;

namespace Desktop.View
{
    public partial class MainEmptyPage : Page
    {
        public MainEmptyPage()
        {
            InitializeComponent();

            if (UserRepository.CurrentUser != null)
            {
                UserNameText.Text = UserRepository.CurrentUser.Name;
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddTaskPage());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            AuthDatabase.Clear();
            ApiService.Reset();
            UserRepository.Logout();
            NavigationService.Navigate(new LoginPage());
        }
    }
}
