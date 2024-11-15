namespace GymApp.Models;

public class AuthModel(string jwt, int userId)
{
    public string Jwt { get; set; } = jwt;
    public int UserId { get; set; } = userId;
}