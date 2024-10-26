using GymApp.Models;
using GymApp.Repositories.UserRepository;
using Microsoft.AspNetCore.Identity;

namespace GymApp.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<object> _hasher;

    public UserService(IUserRepository userRepository) {
        _userRepository = userRepository;
        _hasher = new PasswordHasher<object>();
    }

    public async Task<User> GetUser(string username) {
        return await _userRepository.GetUser(username);
    }

    public async Task<User> LoginUser(string username, string password) {
        // getuser
        User user;
        try {
            user =  user = await _userRepository.GetUser(username);
        } catch {
            throw new Exception("Error fetching user.");
        }

        // check hashpassword
        bool passwordMatches = _hasher.VerifyHashedPassword(null, user.Password, password) == PasswordVerificationResult.Success;

        // if match, logged in
        if (passwordMatches) {
            return user;
        } else {
            throw new ArgumentException("Invalid Password.");
        }
    }

    public async Task<User> RegisterUser(string username, string plainPassword) {
        // TODO: make sure the user does not exist first before registering

        string hashedPassword = _hasher.HashPassword(null , plainPassword);
        User newUser = new User { Id = 1, Username = username, Password = hashedPassword };
        
        try {
            await _userRepository.AddUser(newUser);
        } catch {
            throw new Exception("Error registering user.");
        }

        return newUser;
    }
}