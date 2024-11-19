using GymApp.Models;

namespace GymApp.Services.UserService;

public interface IUserService
{
    Task<User?> GetUser(string username);
    Task<(bool success, string? token, string? error, int? userId)> LoginUser(string username, string password);
    Task<(bool success, string? token, string? error, int? userId)> RegisterUser(string username, string password);
}