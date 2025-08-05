using ExerciseTracker.Console.Models;

namespace ExerciseTracker.Console.Repositories;

public interface IExerciseRepository
{
    Task<List<Exercise>> GetAllAsync();
    Task<Exercise?> GetByIdAsync(int id);
    Task AddAsync(Exercise exercise);
    Task UpdateAsync(Exercise exercise);
    Task DeleteAsync(int id);
}
