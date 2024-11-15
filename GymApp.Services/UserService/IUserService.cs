using GymApp.Models;

namespace GymApp.Services.UserService;

public interface IUserService
{
    Task<User> GetUser(string username);
    Task<Dictionary<string, object>> LoginUser(string username, string password);
    Task<Dictionary<string, object>> RegisterUser(string username, string plainPassword);
    bool CheckAuthentication(string jwt, int userId);
}