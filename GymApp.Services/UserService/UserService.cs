using GymApp.Repositories.UserRepository;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using GymApp.Models;

namespace GymApp.Services.UserService;

public class UserService() : IUserService
{
    private readonly IUserRepository _userRepository = new UserRepository();
    private readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();

    // TODO: find a way to inject secret (dotenv equivalent to C#??)
    private readonly string _signingKey = "dummy-secret";

    private string GenerateJWT(User user) {
        // User is logged in for 1 hour after token creation
        DateTime expires = DateTime.UtcNow.AddHours(1);
        var payload = new Dictionary<string, object>
            {
                {"user_id", user.Id},
                {"username", user.Username},
                {"expires", expires}
            };

        return JWT.JsonWebToken.Encode(payload, _signingKey, JWT.JwtHashAlgorithm.HS256);
    }

    // Use this function to check if a user is still "logged in"
    public bool CheckLoggedIn(string jwt, int userId) {
        try {
            var payload = JWT.JsonWebToken.Decode(jwt, _signingKey);
            dynamic obj = JObject.Parse(payload);
            TimeSpan ts = DateTime.UtcNow - DateTime.Parse(obj.expires.ToString());

            if ((obj.user_id == userId) && ts.Days < 1) {
                return true;
            }

            return false;
        } catch (Exception) {
            return false;
        }
    }

    public async Task<string> LoginUser(string username, string password) {
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
            string jwt = GenerateJWT(user);
            return jwt;
        } else {
            throw new ArgumentException("Invalid Password.");
        }
    }

    public async Task<string> RegisterUser(string username, string plainPassword) {
        // check if logged in
        if (_userRepository.GetUser(username) != null) {
            throw new Exception("User already exists.");
        }

        string hashedPassword = _hasher.HashPassword(null , plainPassword);
        var random = new Random();
        int id = random.Next(100_000_000, 1_000_000_000);
        User newUser = new User { Id = id, Username = username, Password = hashedPassword };
        
        try {
            await _userRepository.AddUser(newUser);
        } catch {
            throw new Exception("Error registering user.");
        }

        string jwt = GenerateJWT(newUser);
        return jwt;
    }

}