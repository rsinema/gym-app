using GymApp.Models;

namespace GymApp.Services.UserService;

public interface IUserService
{
    Task<string> LoginUser(string username, string password);
    Task<string> RegisterUser(string username, string plainPassword);
    bool CheckLoggedIn(string jwt, int userId);
}