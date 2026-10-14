using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Desktop.View;

namespace Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RootFrame.Navigated += OnNavigated;
            Loaded += (_, _) => RootFrame.Navigate(new LoginPage());
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
