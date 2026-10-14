using System.Windows.Controls;
using Desktop.Navigation;
using Desktop.ViewModels;

namespace Desktop.View
{
    public partial class MainEmptyPage : Page
    {
        public MainEmptyPage()
        {
            InitializeComponent();
            DataContext = new MainEmptyViewModel(Navigator.Current!);
        }
    }
}
