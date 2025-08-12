
namespace ExerciseTracker.K_MYR;

internal interface IExerciseRepository
{
    IEnumerable<Exercise> GetAll();
    Task<Exercise> AddAsync(ExerciseInsertModel exerciseEntity);
    Task<Exercise> UpdateAsync(Exercise exerciseEntity);
    Task DeleteAsync(Exercise exerciseEntity);
}
