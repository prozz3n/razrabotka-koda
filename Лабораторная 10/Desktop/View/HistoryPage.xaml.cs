using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Desktop.Repository;
using Todo.Entities;

namespace Desktop.View
{
    public partial class HistoryPage : Page
    {
        public HistoryPage()
        {
            InitializeComponent();

            UserModel? user = UserRepository.CurrentUser;
            string login = user?.Login ?? string.Empty;

            ObservableCollection<TaskModel> tasks = TaskRepository.GetForUser(login);
            ICollectionView view = CollectionViewSource.GetDefaultView(tasks);
            view.Filter = o => o is TaskModel task && task.IsDone;

            HistoryList.ItemsSource = view;
            CounterText.Text = $"Выполнено: {view.Cast<object>().Count()}";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                NavigationService.Navigate(new MainPage());
            }
        }
    }
}
