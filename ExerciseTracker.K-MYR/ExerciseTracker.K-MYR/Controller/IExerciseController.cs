namespace ExerciseTracker.K_MYR;

internal interface IExerciseController
{
    internal IEnumerable<Exercise> GetAll();
    internal Task<Exercise> AddAsync(ExerciseInsertModel exerciseEntity);
    internal Task<Exercise> UpdateAsync(Exercise exerciseEntity);
    internal Task DeleteAsync(Exercise exerciseEntity);
}
