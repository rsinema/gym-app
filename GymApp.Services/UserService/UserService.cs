using GymApp.Models;
using GymApp.Repositories.UserRepository;
using GymApp.Services.AuthService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GymApp.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly PasswordHasher<object> _hasher = new();
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        IAuthService authService,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _authService = authService;
        _logger = logger;
    }

    public async Task<(bool success, string? token, string? error, int? userId)> LoginUser(string username, string password)
    {
        try
        {
            var user = await _userRepository.GetUser(username);
            
            if (user == null)
            {
                return (false, null, "User not found", null);
            }

            var passwordMatches =
                _hasher.VerifyHashedPassword(null, user.Password, password) == PasswordVerificationResult.Success;

            if (!passwordMatches)
            {
                return (false, null, "Invalid password", null);
            }

            var token = _authService.GenerateJWT(user);
            return (true, token, null, user.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return (false, null, "Login failed", null);
        }
    }

    public async Task<(bool success, string? token, string? error, int? userId)> RegisterUser(string username, string password)
    {
        try
        {
            var hashedPassword = _hasher.HashPassword(null, password);
            var random = new Random();
            var id = random.Next(100_000_000, 1_000_000_000);
            var newUser = new User(id, username, hashedPassword);

            await _userRepository.AddUser(newUser);
            var token = _authService.GenerateJWT(newUser);
            
            return (true, token, null, newUser.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return (false, null, "Registration failed", null);
        }
    }

    public async Task<User?> GetUser(string username)
    {
        return await _userRepository.GetUser(username);
    }
}