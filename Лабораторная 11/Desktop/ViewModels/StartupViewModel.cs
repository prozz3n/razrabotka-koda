using System.Threading.Tasks;
using Desktop.Repository;
using Desktop.Services;
using Todo.Entities;

namespace Desktop.ViewModels
{
    public class StartupViewModel
    {
        /// <summary>
        /// Определяет стартовый маршрут. Если пользователь не выходил из программы,
        /// вход пропускается (данные берутся из БД).
        /// </summary>
        public async Task<string> GetStartupRouteAsync()
        {
            if (AuthDatabase.TryGetSaved(out SavedAuth saved))
            {
                ApiService.SetToken(saved.Token);
                UserRepository.SetCurrentUser(new UserModel
                {
                    Login = saved.Login,
                    Name = saved.Name
                });

                try
                {
                    var tasks = await ApiService.GetTasksAsync();
                    TaskRepository.Replace(saved.Login, tasks);
                }
                catch
                {
                    // Нет связи — используем локальный кэш.
                }

                return TaskRepository.GetForUser(saved.Login).Count == 0 ? "mainempty" : "main";
            }

            return "login";
        }
    }
}
