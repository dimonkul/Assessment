using BlazorApp.Models;

namespace BlazorApp.Services
{
    public interface IAuthService
    {
        Task<bool> Login(LoginModel loginModel);
        Task Logout();
        Task<bool> Register(RegisterModel registerModel);
        Task<string> GetToken();
    }
}
