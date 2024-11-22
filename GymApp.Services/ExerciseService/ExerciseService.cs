using GymApp.Models;
using GymApp.Repositories.ExerciseRepository;
using System.Data;

namespace GymApp.Services.ExerciseService;
public class ExerciseService(IExerciseRepository exerciseRepository) : IExerciseService
{
    // Null check for exerciseRepository to prevent runtime issues
    private readonly IExerciseRepository _exerciseRepository = exerciseRepository ??
        throw new ArgumentNullException(nameof(exerciseRepository));

    public async Task<Exercise> GetExerciseAsync(int id)
    {
        // Validate exercise ID
        if (id <= 0)
            throw new ArgumentException("Invalid exercise ID");

        try
        {
            return await exerciseRepository.GetExerciseAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception("Error fetching an exercise.", ex);
        }
    }

    public async Task<List<Exercise>> GetAllExercisesAsync(int userId)
    {

        // Validate user ID
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID");

        try
        {
            return await exerciseRepository.GetAllExercisesAsync(userId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception("Error fetching exercises.", ex);
        }
    }

    public async Task<Exercise> AddExerciseAsync(Exercise exercise)
    {
        ExerciseType exerciseType = exercise.Type;

        if (exercise == null)
            throw new ArgumentNullException(nameof (exercise), "Exercise object cannot be null");

        if (string.IsNullOrWhiteSpace(exercise.Name))
            throw new ArgumentException("Exercise object cannot be empty", nameof(exercise));

        if (exercise.UserId <= 0)
            throw new ArgumentException("Invalid userId. It must be greater than 0", nameof(exercise));

        if (exerciseType.Equals(ExerciseType.Lift))
        {
            if (exercise.Reps == null || exercise.Reps < 0)
                throw new ArgumentException("Reps must be provided and non-negative for lift exercises.", nameof(exercise));
            if (exercise.Weight == null || exercise.Weight < 0)
                throw new ArgumentException("Weight must be provided and non-negative for lift exercises.", nameof(exercise));
        }
        else if (exerciseType.Equals(ExerciseType.Cardio))
        {
            if (exercise.Distance == null || exercise.Distance < 0)
                throw new ArgumentException("Distance must be provided and non-negative for cardio exercises.", nameof(exercise));
            if (exercise.Time == null || exercise.Time < 0)
                throw new ArgumentException("Time must be provided and non-negative for cardio exercises.", nameof(exercise));
        }

        try
        {
            return await exerciseRepository.AddExerciseAsync(exercise);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception("Error adding an exercise.", ex); 
        }
    }
    
}

