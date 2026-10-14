using System.Windows;
using System.Windows.Controls;

namespace Desktop.Helpers
{
    /// <summary>
    /// Позволяет привязать пароль PasswordBox к модели представления,
    /// не нарушая правила безопасности WPF.
    /// </summary>
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(string),
                typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnBoundPasswordChanged));

        private static bool _isUpdating;

        public static string GetBoundPassword(DependencyObject obj)
            => (string)obj.GetValue(BoundPasswordProperty);

        public static void SetBoundPassword(DependencyObject obj, string value)
            => obj.SetValue(BoundPasswordProperty, value);

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PasswordBox box)
            {
                return;
            }

            box.PasswordChanged -= OnPasswordChanged;

            if (!_isUpdating)
            {
                box.Password = e.NewValue as string ?? string.Empty;
            }

            box.PasswordChanged += OnPasswordChanged;
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is not PasswordBox box)
            {
                return;
            }

            _isUpdating = true;
            SetBoundPassword(box, box.Password);
            _isUpdating = false;
        }
    }
}
