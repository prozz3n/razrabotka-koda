using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Desktop.Services
{
    /// <summary>
    /// Локальная база данных (SQLite) для хранения токена авторизации.
    /// Позволяет пропускать окно входа при следующем запуске,
    /// если пользователь не выполнял выход.
    /// </summary>
    public static class AuthDatabase
    {
        private static readonly string DbDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Todo");

        private static readonly string DbPath = Path.Combine(DbDirectory, "todo.db");

        private static string ConnectionString => $"Data Source={DbPath}";

        public static void EnsureCreated()
        {
            Directory.CreateDirectory(DbDirectory);

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using SqliteCommand command = connection.CreateCommand();
            command.CommandText =
                @"CREATE TABLE IF NOT EXISTS Auth (
                      Id    INTEGER PRIMARY KEY CHECK (Id = 1),
                      Token TEXT NOT NULL,
                      Login TEXT NOT NULL,
                      Name  TEXT NOT NULL
                  );";
            command.ExecuteNonQuery();
        }

        public static void Save(string token, string login, string name)
        {
            EnsureCreated();

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using SqliteCommand command = connection.CreateCommand();
            command.CommandText =
                @"DELETE FROM Auth;
                  INSERT INTO Auth (Id, Token, Login, Name)
                  VALUES (1, $token, $login, $name);";
            command.Parameters.AddWithValue("$token", token ?? string.Empty);
            command.Parameters.AddWithValue("$login", login ?? string.Empty);
            command.Parameters.AddWithValue("$name", name ?? string.Empty);
            command.ExecuteNonQuery();
        }

        public static bool TryGetSaved(out SavedAuth saved)
        {
            saved = new SavedAuth();

            try
            {
                EnsureCreated();

                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT Token, Login, Name FROM Auth WHERE Id = 1;";

                using SqliteDataReader reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return false;
                }

                saved = new SavedAuth
                {
                    Token = reader.GetString(0),
                    Login = reader.GetString(1),
                    Name = reader.GetString(2)
                };

                return !string.IsNullOrEmpty(saved.Token);
            }
            catch
            {
                return false;
            }
        }

        public static void Clear()
        {
            try
            {
                EnsureCreated();

                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Auth;";
                command.ExecuteNonQuery();
            }
            catch
            {
                // Игнорируем ошибки очистки.
            }
        }
    }

    public sealed class SavedAuth
    {
        public string Token { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}
