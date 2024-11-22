using GymApp.Repositories.UserRepository;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using GymApp.Models;

namespace GymApp.Services.UserService;

public class UserService(IUserRepository userRepository, string signingKey) : IUserService
{
    private readonly PasswordHasher<object> _hasher = new();

    private string GenerateJWT(User user)
    {
        // User is logged in for 1 hour after token creation
        var expires = DateTime.UtcNow.AddHours(1);
        var payload = new Dictionary<string, object>
            {
                {"user_id", user.Id},
                {"username", user.Username},
                {"expires", expires}
            };

        return JWT.JsonWebToken.Encode(payload, signingKey, JWT.JwtHashAlgorithm.HS256);
    }

    // Use this function to check if a user is still "logged in"
    public bool CheckAuthentication(string jwt, int userId)
    {
        try
        {
            var payload = JWT.JsonWebToken.Decode(jwt, signingKey);
            dynamic obj = JObject.Parse(payload);

            // Ensure the token is not expired
            if (DateTime.UtcNow > DateTime.Parse(obj.expires.ToString()))
                return false;

            // Check user ID
            return obj.user_id == userId;
        } 
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<Dictionary<string, object>> LoginUser(string username, string password) 
    {
        User user;
        try 
        {
            user = await userRepository.GetUser(username);
        } 
        catch 
        {
            throw new Exception("Error fetching user.");
        }
        
        var passwordMatches = 
            _hasher.VerifyHashedPassword(signingKey, user.Password, password) == PasswordVerificationResult.Success;

        if (!passwordMatches) throw new ArgumentException("Invalid Password.");
        var result = new Dictionary<string, object>
        {
            {"user_id", user.Id},
            {"jwt", GenerateJWT(user)}
        };
            
        return result;

    }

    public async Task<Dictionary<string, object>> RegisterUser(string username, string plainPassword)
    {
        var hashedPassword = _hasher.HashPassword(signingKey , plainPassword);
        var random = new Random();
        var id = random.Next(100_000_000, 1_000_000_000);
        var newUser = new User(id, username, hashedPassword);

        if (await userRepository.UserExistsAsync(username))
            throw new ArgumentException($"User with username '{username}' already exists.");

        try
        {
            await userRepository.AddUser(newUser);

            var result = new Dictionary<string, object>
            {
                { "user_id", newUser.Id },
                { "jwt", GenerateJWT(newUser) }
            };
            
            return result;
        } 
        catch (Exception ex)
        {
            throw new Exception("Error registering user.", ex);
        }
    }
    
    public async Task<User> GetUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username must be provided");

        try
        {
            return await userRepository.GetUser(username);
        }
        catch (Exception ex)
        {
            throw new Exception("Error getting user.", ex);
        }
    }
}