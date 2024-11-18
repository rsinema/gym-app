using GymApp.Models;
using MongoDB.Driver;

namespace GymApp.Repositories.ExerciseRepository;

public class ExerciseRepository : IExerciseRepository
{
    private readonly IMongoCollection<Exercise> _exerciseCollection;

    public ExerciseRepository(string connString)
    {
        var client = new MongoClient(connString);
        var database = client.GetDatabase("gym-app");
        _exerciseCollection = database.GetCollection<Exercise>("exercise");
    }
    
    public async Task<Exercise> GetExerciseAsync(int id)
    {
        var filter = Builders<Exercise>.Filter.Eq(x => x.Id, id);
        return await _exerciseCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<Exercise>> GetAllExercisesAsync(int userId)
    {
        var filter = Builders<Exercise>.Filter.Eq(u => u.UserId, userId);
        return await _exerciseCollection.Find(filter).ToListAsync();
    }

    public async Task<Exercise> AddExerciseAsync(Exercise exercise)
    {
        await _exerciseCollection.InsertOneAsync(exercise);
        return exercise;
    }
}