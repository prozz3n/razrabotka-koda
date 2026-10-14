using System.Windows.Controls;
using Desktop.Navigation;
using Desktop.ViewModels;

namespace Desktop.View
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainViewModel(Navigator.Current!);
        }
    }
}
