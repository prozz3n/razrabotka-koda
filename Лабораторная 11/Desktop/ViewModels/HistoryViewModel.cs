using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Desktop.Navigation;
using Desktop.Repository;
using Todo.Entities;

namespace Desktop.ViewModels
{
    public class HistoryViewModel : ObservableObject
    {
        private readonly INavigationService _navigation;

        public HistoryViewModel(INavigationService navigation)
        {
            _navigation = navigation;

            string login = UserRepository.CurrentUser?.Login ?? string.Empty;
            ObservableCollection<TaskModel> tasks = TaskRepository.GetForUser(login);

            ICollectionView view = CollectionViewSource.GetDefaultView(tasks);
            view.Filter = o => o is TaskModel task && task.IsDone;
            Tasks = view;

            Counter = $"Выполнено: {view.Cast<object>().Count()}";
            BackCommand = new RelayCommand(Back);
        }

        public ICollectionView Tasks { get; }

        public string Counter { get; }

        public RelayCommand BackCommand { get; }

        private void Back()
        {
            if (_navigation.CanGoBack)
            {
                _navigation.GoBack();
            }
            else
            {
                _navigation.Navigate("main");
            }
        }
    }
}
