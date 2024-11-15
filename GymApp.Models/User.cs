namespace GymApp.Models;

public class User(int id, string username, string password)
{
    public int Id { get; } = id;
    public string Username { get; } = username;
    public string Password { get; } = password;
}
