using System.Windows;
using Desktop.Repository;

namespace Desktop
{
    public partial class MainEmptyWindow : Window
    {
        public MainEmptyWindow()
        {
            InitializeComponent();

            if (UserRepository.CurrentUser != null)
            {
                UserNameText.Text = UserRepository.CurrentUser.Name;
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            UserRepository.Logout();
            new LoginWindow().Show();
            Close();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            // Логика добавления задачи будет реализована в лабораторных работах 4–5.
        }
    }
}
