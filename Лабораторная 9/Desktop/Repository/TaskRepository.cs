using System.Collections.Generic;
using System.Collections.ObjectModel;
using Todo.Entities;

// Локальный кэш задач; синхронизируется с API (см. Desktop.Services.ApiService).

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

        public static void Replace(string login, IEnumerable<TaskModel> tasks)
        {
            ObservableCollection<TaskModel> list = GetForUser(login);
            list.Clear();

            foreach (TaskModel task in tasks)
            {
                task.OwnerLogin = login;
                list.Add(task);
            }
        }
    }
}
