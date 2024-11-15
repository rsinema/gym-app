using GymApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.Services.ExerciseService;
public class ExerciseService(IExerciseRepository exerciseRepository) : IExerciseService
{
    public async Task<Exercise> GetExerciseAsync(int id)
    {
        return await exerciseRepository.GetExerciseAsync(id);
    }

    public async Task<List<Exercise>> GetAllExercisesAsync(int userId)
    {
        return await exerciseRepository.GetAllExercisesAsync(userId);
    }

    public async Task<Exercise> AddExerciseAsync(Exercise exercise)
    {
        return await exerciseRepository.AddExerciseAsync(exercise);
    }





}

