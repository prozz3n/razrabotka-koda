using Desktop.Validation;
using Xunit;

namespace Todo.Tests
{
    public class InputValidatorTests
    {
        [Theory]
        [InlineData("user@mail.com")]
        [InlineData("a.b-c@sub.domain.ru")]
        [InlineData("test123@site.org")]
        public void IsValidEmail_ValidAddresses_ReturnsTrue(string email)
        {
            Assert.True(InputValidator.IsValidEmail(email));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("usermail.com")]
        [InlineData("user@mail")]
        [InlineData("@mail.com")]
        [InlineData("user@.com")]
        [InlineData("user mail@site.com")]
        public void IsValidEmail_InvalidAddresses_ReturnsFalse(string? email)
        {
            Assert.False(InputValidator.IsValidEmail(email));
        }

        [Theory]
        [InlineData("secret", true)]
        [InlineData("123456", true)]
        [InlineData("verylongpassword", true)]
        [InlineData("12345", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsValidPassword_ChecksMinimumLength(string? password, bool expected)
        {
            Assert.Equal(expected, InputValidator.IsValidPassword(password));
        }

        [Theory]
        [InlineData("Иван", true)]
        [InlineData("Bob", true)]
        [InlineData("  Ann  ", true)]
        [InlineData("Al", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsValidName_ChecksMinimumLength(string? name, bool expected)
        {
            Assert.Equal(expected, InputValidator.IsValidName(name));
        }
    }
}
