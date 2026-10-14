using System;
using System.Windows.Controls;
using Desktop.View;

namespace Desktop.Navigation
{
    public class FrameNavigationService : INavigationService
    {
        private readonly Frame _frame;

        public FrameNavigationService(Frame frame)
        {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        }

        public bool CanGoBack => _frame.CanGoBack;

        public void Navigate(string route)
        {
            Page page = CreatePage(route);
            _frame.Navigate(page);
        }

        public void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }

        private static Page CreatePage(string route)
        {
            return route switch
            {
                "registration" => new RegistrationPage(),
                "main" => new MainPage(),
                "mainempty" => new MainEmptyPage(),
                "addtask" => new AddTaskPage(),
                "history" => new HistoryPage(),
                _ => new LoginPage()
            };
        }
    }
}
