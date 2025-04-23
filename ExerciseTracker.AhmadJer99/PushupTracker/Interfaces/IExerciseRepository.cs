using ExerciseTracker.Models;

namespace ExerciseTracker.Interfaces;

public interface IExerciseRepository
{
    Task<Pushup> GetByIdAsync(int id);
    Task<IEnumerable<Pushup>> GetAllAsync();
    Task AddAsync(Pushup entity);
    Task UpdateAsync(Pushup entity);
    Task DeleteAsync(Pushup entity);
}
