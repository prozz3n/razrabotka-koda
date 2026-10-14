using System.Windows;

namespace Desktop
{
    public partial class MainEmptyWindow : Window
    {
        public MainEmptyWindow()
        {
            InitializeComponent();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            // Логика добавления задачи будет реализована в лабораторных работах 4–5.
        }
    }
}
