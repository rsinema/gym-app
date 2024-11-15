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
    public bool CheckLoggedIn(string jwt, int userId)
    {
        try
        {
            var payload = JWT.JsonWebToken.Decode(jwt, signingKey);
            dynamic obj = JObject.Parse(payload);
            TimeSpan ts = DateTime.UtcNow - DateTime.Parse(obj.expires.ToString());

            return (obj.user_id == userId) && ts.Days < 1;
        } 
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<string> LoginUser(string username, string password) 
    {
        // getuser
        User user;
        try 
        {
            user = await userRepository.GetUser(username);
        } 
        catch 
        {
            throw new Exception("Error fetching user.");
        }

        // check hashpassword
        var passwordMatches = 
            _hasher.VerifyHashedPassword(signingKey, user.Password, password) == PasswordVerificationResult.Success;

        // if match, logged in
        if (passwordMatches)
        {
            return GenerateJWT(user);
        } 
        else 
        {
            throw new ArgumentException("Invalid Password.");
        }
    }

    public async Task<string> RegisterUser(string username, string plainPassword)
    {
        // check if logged in
        if (userRepository.GetUser(username) != null) throw new Exception("User already exists.");

        var hashedPassword = _hasher.HashPassword(signingKey , plainPassword);
        var random = new Random();
        var id = random.Next(100_000_000, 1_000_000_000);
        var newUser = new User(id, username, hashedPassword);
        
        try 
        {
            await userRepository.AddUser(newUser);
            var jwt = GenerateJWT(newUser);
            return jwt;
        } 
        catch 
        {
            throw new Exception("Error registering user.");
        }
    }

    // TODO: fix hardcoded value
    public Task<User> GetUser()
    {
        return Task.FromResult(new User("Riley", "password"));
    }
}