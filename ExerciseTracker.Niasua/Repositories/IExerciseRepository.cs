using ExerciseTracker.Niasua.Models;

namespace ExerciseTracker.Niasua.Repositories;

public interface IExerciseRepository
{
    Task<List<Exercise>> GetAllAsync();
    Task<Exercise?> GetByIdAsync(int id);
    Task AddAsync(Exercise exercise);
    Task UpdateAsync(Exercise exercise);
    Task DeleteAsync(int id);
}
