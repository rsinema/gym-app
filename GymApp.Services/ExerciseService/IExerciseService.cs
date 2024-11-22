using GymApp.Models;

namespace GymApp.Services.ExerciseService
{
    internal interface IExerciseService
    {
        Task<Exercise> GetExerciseAsync(int id);

        Task<List<Exercise>> GetAllExercisesAsync(int userId);

        Task<Exercise> AddExerciseAsync(Exercise exercise);


    }
}
