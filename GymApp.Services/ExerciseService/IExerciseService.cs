using GymApp.Models;

namespace GymApp.Services.ExerciseService
{
    public interface IExerciseService
    {
        Task<Exercise?> GetExerciseAsync(int id);

        Task<List<Exercise>?> GetAllExercisesAsync(int userId);

        Task<Exercise?> AddExerciseAsync(Exercise exercise);


    }
}
