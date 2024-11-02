namespace GymApp.Models;

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }

    public User(string username, string password)
    {
        Password = password;
        Username = username;
    }
}
