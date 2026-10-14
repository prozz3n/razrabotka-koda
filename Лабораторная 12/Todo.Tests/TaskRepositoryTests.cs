using System;
using System.Collections.Generic;
using Desktop.Repository;
using Todo.Entities;
using Xunit;

namespace Todo.Tests
{
    public class TaskRepositoryTests
    {
        private static string NewLogin() => $"user_{Guid.NewGuid():N}@mail.com";

        [Fact]
        public void Add_ThenGetForUser_ContainsTask()
        {
            string login = NewLogin();
            var task = new TaskModel { Title = "Задача" };

            TaskRepository.Add(login, task);

            Assert.Contains(task, TaskRepository.GetForUser(login));
            Assert.Equal(login, task.OwnerLogin);
        }

        [Fact]
        public void Remove_DeletesTask()
        {
            string login = NewLogin();
            var task = new TaskModel { Title = "Задача" };
            TaskRepository.Add(login, task);

            TaskRepository.Remove(login, task);

            Assert.DoesNotContain(task, TaskRepository.GetForUser(login));
        }

        [Fact]
        public void Replace_ClearsAndFillsTasks()
        {
            string login = NewLogin();
            TaskRepository.Add(login, new TaskModel { Title = "Старая" });

            var fresh = new List<TaskModel>
            {
                new TaskModel { Title = "Первая" },
                new TaskModel { Title = "Вторая" }
            };

            TaskRepository.Replace(login, fresh);

            var result = TaskRepository.GetForUser(login);
            Assert.Equal(2, result.Count);
            Assert.All(result, t => Assert.Equal(login, t.OwnerLogin));
        }

        [Fact]
        public void GetForUser_UnknownLogin_ReturnsEmptyCollection()
        {
            var tasks = TaskRepository.GetForUser(NewLogin());

            Assert.Empty(tasks);
        }
    }
}
