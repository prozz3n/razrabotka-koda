using System;

namespace Todo.Entities
{
    public class UserModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; } = DateTime.Now;
    }
}
