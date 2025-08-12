using ExerciseTracker.Models;
using ExerciseTracker.Repository;
using static ExerciseTracker.Models.Enums;

namespace ExerciseTracker.Service;

public interface IService
{
    Task<List<Exercise>> GetAllExercisesAsync();
    Task<Exercise?> GetExerciseByIdAsync(int id);
    Task AddExerciseAsync(DateTime dateStart, DateTime dateEnd, string comments, ExerciseType exerciseType);
    Task UpdateExerciseAsync(int id, DateTime dateStart, DateTime dateEnd, string comments, ExerciseType exerciseType);
    Task DeleteExerciseByIdAsync(int id);
}