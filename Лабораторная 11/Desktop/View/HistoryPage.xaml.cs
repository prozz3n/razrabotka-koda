using System.Windows.Controls;
using Desktop.Navigation;
using Desktop.ViewModels;

namespace Desktop.View
{
    public partial class HistoryPage : Page
    {
        public HistoryPage()
        {
            InitializeComponent();
            DataContext = new HistoryViewModel(Navigator.Current!);
        }
    }
}
