using ExerciseTracker.Models;

namespace ExerciseTracker.Repository;

public interface IRepository
{
    Task<List<Exercise>> GetAllExercisesAsync();
    Task<Exercise?> GetExerciseByIdAsync(int id);
    Task AddExerciseAsync(Exercise exercise);
    Task DeleteExerciseByIdAsync(int id);
    Task UpdateExerciseAsync(Exercise exercise);
}