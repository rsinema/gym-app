using GymApp.Models;

namespace GymApp.Services.UserService;

public interface IUserService
{
    Task<User> GetUser();
    Task<bool> LoginUser(string username, string password);
    Task<string> RegisterUser(string username, string plainPassword);
    bool CheckLoggedIn(string jwt, int userId);
}