using GymApp.Models;

namespace GymApp.Services.UserService;

public interface IUserService
{
    Task<User> GetUser(string username);
    Task<AuthModel> LoginUser(string username, string password);
    Task<AuthModel> RegisterUser(string username, string plainPassword);
    bool CheckAuthentication(string jwt, int userId);
}