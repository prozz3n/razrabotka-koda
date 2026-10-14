using System.Collections.Generic;
using System.Collections.ObjectModel;
using Todo.Entities;

namespace Desktop.Repository
{
    public static class TaskRepository
    {
        private static readonly Dictionary<string, ObservableCollection<TaskModel>> Store =
            new Dictionary<string, ObservableCollection<TaskModel>>();

        public static ObservableCollection<TaskModel> GetForUser(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                login = string.Empty;
            }

            if (!Store.TryGetValue(login, out ObservableCollection<TaskModel>? list))
            {
                list = new ObservableCollection<TaskModel>();
                Store[login] = list;
            }

            return list;
        }

        public static void Add(string login, TaskModel task)
        {
            task.OwnerLogin = login;
            GetForUser(login).Add(task);
        }

        public static void Remove(string login, TaskModel task)
        {
            GetForUser(login).Remove(task);
        }
    }
}
