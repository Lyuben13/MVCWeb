using MVC.Intro.Models;
using MVC.Intro.ViewModels;

namespace MVC.Intro.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(RegisterViewModel model);

        Task<User?> LoginAsync(string email, string password);

        Task<bool> EmailExistsAsync(string email);
    }
}