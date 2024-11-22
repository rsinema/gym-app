using MongoDB.Driver;
using GymApp.Models;

namespace GymApp.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<User> _userCollection;

    public UserRepository(string connString)
    {
        _client = new MongoClient(connString);
        _database = _client.GetDatabase("gym-app");
        _userCollection = _database.GetCollection<User>("user");
    }
    public async Task<bool> UserExistsAsync(string username)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Username, username);
        var user = await _userCollection.Find(filter).FirstOrDefaultAsync();
        return user != null;
    }

    public async Task<User> GetUser(string username)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Username, username);
        User user = await _userCollection.Find(filter).FirstOrDefaultAsync();
        
        return user;
    }

    public async Task AddUser(User user)
    {
        await _userCollection.InsertOneAsync(user);
    }
}
