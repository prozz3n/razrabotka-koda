using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Entities;

namespace Desktop.Repository
{
    public static class UserRepository
    {
        private static readonly List<UserModel> Users = new List<UserModel>();

        public static UserModel? CurrentUser { get; private set; }

        public static bool Register(UserModel user, out string error)
        {
            error = string.Empty;

            if (user == null)
            {
                error = "Некорректные данные пользователя.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.Login))
            {
                error = "Логин не может быть пустым.";
                return false;
            }

            bool exists = Users.Any(u =>
                string.Equals(u.Login, user.Login, StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                error = "Пользователь с таким логином (email) уже зарегистрирован.";
                return false;
            }

            Users.Add(user);
            return true;
        }

        public static UserModel? Authorize(string login, string password)
        {
            UserModel? user = Users.FirstOrDefault(u =>
                string.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);

            if (user != null)
            {
                CurrentUser = user;
            }

            return user;
        }

        public static void SetCurrentUser(UserModel user)
        {
            CurrentUser = user;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}
