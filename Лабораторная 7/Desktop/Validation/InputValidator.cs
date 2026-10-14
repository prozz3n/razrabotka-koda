using System.Text.RegularExpressions;

namespace Desktop.Validation
{
    public static class InputValidator
    {
        private static readonly Regex EmailPattern =
            new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        public static bool IsValidEmail(string? email)
        {
            return !string.IsNullOrWhiteSpace(email) && EmailPattern.IsMatch(email);
        }

        public static bool IsValidPassword(string? password)
        {
            return !string.IsNullOrEmpty(password) && password.Length >= 6;
        }

        public static bool IsValidName(string? name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Trim().Length >= 3;
        }
    }
}
