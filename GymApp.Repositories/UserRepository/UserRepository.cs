using GymApp.Models;

namespace GymApp.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
    public UserRepository()
    {
    }

    public User GetUser()
    {
        return new User { Id = 1, Name = "Riley"};
    }
}
