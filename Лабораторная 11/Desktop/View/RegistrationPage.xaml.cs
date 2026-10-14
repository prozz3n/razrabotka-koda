using System.Windows.Controls;
using Desktop.Navigation;
using Desktop.ViewModels;

namespace Desktop.View
{
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
            DataContext = new RegistrationViewModel(Navigator.Current!);
        }
    }
}
