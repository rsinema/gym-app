using GymApp.Models;

namespace GymApp.Services.UserService;

public interface IUserService
{
    Task<User> GetUser(string username);
    Task<User> LoginUser(string username, string password);
    Task<User> RegisterUser(string username, string plainPassword);
}