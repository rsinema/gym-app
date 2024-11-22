using GymApp.Models;

namespace GymApp.Repositories.UserRepository;

public interface IUserRepository
{
    Task<bool> UserExistsAsync(string username);
    Task<User> GetUser(string username);
    Task AddUser(User user);
}
