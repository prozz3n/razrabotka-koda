using System.Windows.Controls;
using Desktop.Navigation;
using Desktop.ViewModels;

namespace Desktop.View
{
    public partial class AddTaskPage : Page
    {
        public AddTaskPage()
        {
            InitializeComponent();
            DataContext = new AddTaskViewModel(Navigator.Current!);
        }
    }
}
