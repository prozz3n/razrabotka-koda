using System.Windows.Controls;
using Desktop.Navigation;
using Desktop.ViewModels;

namespace Desktop.View
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            DataContext = new LoginViewModel(Navigator.Current!);
        }
    }
}
