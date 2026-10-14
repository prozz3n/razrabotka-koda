namespace Desktop.Navigation
{
    public interface INavigationService
    {
        bool CanGoBack { get; }

        void Navigate(string route);

        void GoBack();
    }
}
