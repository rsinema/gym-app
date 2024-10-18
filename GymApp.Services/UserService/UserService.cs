using GymApp.Models;
using GymApp.Repositories.UserRepository;

namespace GymApp.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User GetUser()
    {
        return _userRepository.GetUser();
    }
}