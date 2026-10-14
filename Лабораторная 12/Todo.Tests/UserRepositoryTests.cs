using System;
using Desktop.Repository;
using Todo.Entities;
using Xunit;

namespace Todo.Tests
{
    public class UserRepositoryTests
    {
        private static UserModel NewUser(string? login = null, string password = "secret1")
        {
            return new UserModel
            {
                Name = "Тестовый пользователь",
                Login = login ?? $"user_{Guid.NewGuid():N}@mail.com",
                Password = password
            };
        }

        [Fact]
        public void Register_NewUser_ReturnsTrue()
        {
            UserModel user = NewUser();

            bool result = UserRepository.Register(user, out string error);

            Assert.True(result);
            Assert.Equal(string.Empty, error);
        }

        [Fact]
        public void Register_DuplicateLogin_ReturnsFalse()
        {
            UserModel user = NewUser();
            UserRepository.Register(user, out _);

            bool result = UserRepository.Register(NewUser(user.Login), out string error);

            Assert.False(result);
            Assert.False(string.IsNullOrWhiteSpace(error));
        }

        [Fact]
        public void Register_EmptyLogin_ReturnsFalse()
        {
            UserModel user = NewUser("   ");

            bool result = UserRepository.Register(user, out string error);

            Assert.False(result);
            Assert.False(string.IsNullOrWhiteSpace(error));
        }

        [Fact]
        public void Authorize_CorrectCredentials_ReturnsUserAndSetsCurrent()
        {
            UserModel user = NewUser(password: "qwerty");
            UserRepository.Register(user, out _);

            UserModel? authorized = UserRepository.Authorize(user.Login, "qwerty");

            Assert.NotNull(authorized);
            Assert.Same(user, UserRepository.CurrentUser);
        }

        [Fact]
        public void Authorize_WrongPassword_ReturnsNull()
        {
            UserModel user = NewUser(password: "qwerty");
            UserRepository.Register(user, out _);

            UserModel? authorized = UserRepository.Authorize(user.Login, "wrong");

            Assert.Null(authorized);
        }

        [Fact]
        public void Logout_ClearsCurrentUser()
        {
            UserModel user = NewUser();
            UserRepository.SetCurrentUser(user);

            UserRepository.Logout();

            Assert.Null(UserRepository.CurrentUser);
        }
    }
}
