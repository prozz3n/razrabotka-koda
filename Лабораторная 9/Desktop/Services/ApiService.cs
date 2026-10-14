using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Todo.Entities;

namespace Desktop.Services
{
    /// <summary>
    /// Клиент для работы с API (http://45.144.64.179/swagger/index.html).
    /// Токен авторизации хранится в статической переменной.
    /// Пути эндпоинтов при необходимости скорректируйте под актуальный swagger.
    /// </summary>
    public static class ApiService
    {
        public const string BaseAddress = "http://45.144.64.179/";

        private const string RegisterPath = "api/Auth/register";
        private const string LoginPath = "api/Auth/login";
        private const string TasksPath = "api/Tasks";

        private static readonly HttpClient Client = new HttpClient
        {
            BaseAddress = new Uri(BaseAddress),
            Timeout = TimeSpan.FromSeconds(20)
        };

        public static string? Token { get; private set; }

        public static bool IsAuthorized => !string.IsNullOrEmpty(Token);

        public static void SetToken(string? token)
        {
            Token = token;

            Client.DefaultRequestHeaders.Authorization =
                string.IsNullOrEmpty(token)
                    ? null
                    : new AuthenticationHeaderValue("Bearer", token);
        }

        public static void Reset()
        {
            SetToken(null);
        }

        public static async Task<AuthResult> RegisterAsync(string name, string login, string password)
        {
            var body = new { Name = name, Login = login, Password = password };

            using HttpResponseMessage response =
                await Client.PostAsJsonAsync(RegisterPath, body);

            if (!response.IsSuccessStatusCode)
            {
                return AuthResult.Fail(await ReadErrorAsync(response));
            }

            AuthResponse? auth = await TryReadAuthAsync(response);
            SetToken(auth?.Token);

            return AuthResult.Ok(new UserModel { Name = name, Login = login, Password = password });
        }

        public static async Task<AuthResult> LoginAsync(string login, string password)
        {
            var body = new { Login = login, Password = password };

            using HttpResponseMessage response =
                await Client.PostAsJsonAsync(LoginPath, body);

            if (!response.IsSuccessStatusCode)
            {
                return AuthResult.Fail(await ReadErrorAsync(response));
            }

            AuthResponse? auth = await TryReadAuthAsync(response);
            SetToken(auth?.Token);

            var user = new UserModel
            {
                Name = string.IsNullOrWhiteSpace(auth?.Name) ? login : auth!.Name!,
                Login = login
            };

            return AuthResult.Ok(user);
        }

        public static async Task<List<TaskModel>> GetTasksAsync()
        {
            List<TaskModel>? tasks = await Client.GetFromJsonAsync<List<TaskModel>>(TasksPath);
            return tasks ?? new List<TaskModel>();
        }

        public static async Task<TaskModel?> CreateTaskAsync(TaskModel task)
        {
            using HttpResponseMessage response =
                await Client.PostAsJsonAsync(TasksPath, task);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TaskModel>();
        }

        public static async Task<bool> UpdateTaskAsync(TaskModel task)
        {
            using HttpResponseMessage response =
                await Client.PutAsJsonAsync($"{TasksPath}/{task.Id}", task);

            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> DeleteTaskAsync(Guid id)
        {
            using HttpResponseMessage response =
                await Client.DeleteAsync($"{TasksPath}/{id}");

            return response.IsSuccessStatusCode;
        }

        private static async Task<AuthResponse?> TryReadAuthAsync(HttpResponseMessage response)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<AuthResponse>();
            }
            catch
            {
                return null;
            }
        }

        private static async Task<string> ReadErrorAsync(HttpResponseMessage response)
        {
            try
            {
                string text = await response.Content.ReadAsStringAsync();
                return string.IsNullOrWhiteSpace(text)
                    ? $"Ошибка {(int)response.StatusCode}"
                    : text;
            }
            catch
            {
                return $"Ошибка {(int)response.StatusCode}";
            }
        }
    }

    public sealed class AuthResponse
    {
        public string? Token { get; set; }

        public string? Name { get; set; }
    }

    public sealed class AuthResult
    {
        public bool Success { get; private set; }

        public string Error { get; private set; } = string.Empty;

        public UserModel? User { get; private set; }

        public static AuthResult Ok(UserModel user) =>
            new AuthResult { Success = true, User = user };

        public static AuthResult Fail(string error) =>
            new AuthResult { Success = false, Error = error };
    }
}
