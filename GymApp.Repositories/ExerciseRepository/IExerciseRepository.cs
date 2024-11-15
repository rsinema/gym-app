using GymApp.Models;

namespace GymApp.Repositories.ExerciseRepository;

public interface IExerciseRepository
{
    Task<Exercise> GetExerciseAsync(int id);
    Task<List<Exercise>> GetAllExercisesAsync(int userId);
    Task<Exercise> AddExerciseAsync(Exercise exercise);
}