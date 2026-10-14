using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Desktop.Repository;
using Desktop.Services;
using Desktop.View;
using Todo.Entities;

namespace Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RootFrame.Navigated += OnNavigated;
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Если пользователь не выходил из программы — пропускаем вход.
            if (AuthDatabase.TryGetSaved(out SavedAuth saved))
            {
                ApiService.SetToken(saved.Token);
                UserRepository.SetCurrentUser(new UserModel
                {
                    Login = saved.Login,
                    Name = saved.Name
                });

                try
                {
                    var tasks = await ApiService.GetTasksAsync();
                    TaskRepository.Replace(saved.Login, tasks);
                }
                catch
                {
                    // Нет связи — используем локальный кэш.
                }

                NavigateToMain(saved.Login);
                return;
            }

            RootFrame.Navigate(new LoginPage());
        }

        private void NavigateToMain(string login)
        {
            if (TaskRepository.GetForUser(login).Count == 0)
            {
                RootFrame.Navigate(new MainEmptyPage());
            }
            else
            {
                RootFrame.Navigate(new MainPage());
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is not UIElement content)
            {
                return;
            }

            var slide = new TranslateTransform(48, 0);
            content.RenderTransform = slide;
            content.Opacity = 0;

            var duration = TimeSpan.FromMilliseconds(350);
            var ease = new CubicEase { EasingMode = EasingMode.EaseOut };

            content.BeginAnimation(OpacityProperty,
                new DoubleAnimation(0, 1, duration) { EasingFunction = ease });

            slide.BeginAnimation(TranslateTransform.XProperty,
                new DoubleAnimation(48, 0, duration) { EasingFunction = ease });
        }
    }
}
