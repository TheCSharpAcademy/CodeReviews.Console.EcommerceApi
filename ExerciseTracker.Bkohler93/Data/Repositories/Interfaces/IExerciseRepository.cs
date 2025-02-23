using Data.Entities;

namespace Data.Repositories;

public interface IExerciseRepository : IRepository<Exercise>
{
    Task<Exercise?> GetExerciseByIdAsync(int id);

    Task<List<Exercise>> GetAllExercisesAsync();
}