using GymApp.Models;

namespace GymApp.Services.AuthService;

public interface IAuthService
{
    string GenerateJWT(User user);
    bool ValidateToken(string token);
}