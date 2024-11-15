using GymApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.Services.ExerciseService
{
    internal interface IExerciseService
    {
        Task<Exercise> GetExerciseAsync(int id);

        Task<List<Exercise>> GetAllExercisesAsync(int userId);

        Task<Exercise> AddExerciseAsync(Exercise exercise);


    }
}
