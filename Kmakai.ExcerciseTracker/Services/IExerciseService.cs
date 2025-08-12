using Kmakai.ExerciseTracker.Models;

namespace Kmakai.ExerciseTracker.Services;

public interface IExerciseService
{
    Task<Exercise> AddAsync(Exercise entity);
    Task<Exercise> DeleteAsync(int id);
    Task<IEnumerable<Exercise>> GetAllAsync();
    Task<Exercise> GetAsync(int id);
    Task<Exercise> UpdateAsync(Exercise entity);
}