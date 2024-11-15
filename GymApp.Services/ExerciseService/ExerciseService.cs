using GymApp.Models;
using GymApp.Repositories.ExerciseRepository;

namespace GymApp.Services.ExerciseService;
public class ExerciseService(IExerciseRepository exerciseRepository) : IExerciseService
{
    public async Task<Exercise> GetExerciseAsync(int id)
    {
        try
        {
            return await exerciseRepository.GetExerciseAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<Exercise>?> GetAllExercisesAsync(int userId)
    {
        try
        {
            return await exerciseRepository.GetAllExercisesAsync(userId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception(ex.Message);
        }
    }

    public async Task<Exercise?> AddExerciseAsync(Exercise exercise)
    {
        try
        {
            return await exerciseRepository.AddExerciseAsync(exercise);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception(ex.Message);
        }
    }
    
}

