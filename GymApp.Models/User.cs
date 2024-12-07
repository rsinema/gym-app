namespace GymApp.Models;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class User(int id, string username, string password)
{
    [BsonId]
    [BsonRepresentation(BsonType.Int32)]
    public int Id { get; set; } = id;
    public string Username { get; set; } = username;
    public string Password { get; set; } = password;
}
