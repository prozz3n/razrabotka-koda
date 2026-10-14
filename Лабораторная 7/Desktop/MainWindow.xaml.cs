using System.Windows;
using Desktop.View;

namespace Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (_, _) => RootFrame.Navigate(new LoginPage());
        }
    }
}
