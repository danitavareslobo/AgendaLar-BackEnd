using AgendaLarAPI.Models.User.ViewModels;

namespace AgendaLarAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginResponse?> RegisterUserAsync(UserRegister userRegister);
        Task<UserLoginResponse?> Login(UserLogin userLogin);
    }
}
