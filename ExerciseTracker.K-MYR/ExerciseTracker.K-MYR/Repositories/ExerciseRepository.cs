namespace ExerciseTracker.K_MYR;

internal class ExerciseRepository : IExerciseRepository
{
    private readonly ExerciseDbContext _ExerciseDbContext;

    public ExerciseRepository(ExerciseDbContext exerciseDbContext)
    {
        _ExerciseDbContext = exerciseDbContext;
    }

    public IEnumerable<Exercise> GetAll()
    {
        try
        {
            return _ExerciseDbContext.Set<Exercise>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't retrieve Entities: {ex.Message}");
        }
    }

    public async Task<Exercise> AddAsync(ExerciseInsertModel exerciseEntity)
    {
        ArgumentNullException.ThrowIfNull(exerciseEntity);

        try
        {
            var exercise = new Exercise()
            {
                Type = exerciseEntity.Type,
                StartTime = exerciseEntity.StartTime,
                EndTime = exerciseEntity.EndTime,
                Duration = (exerciseEntity.EndTime - exerciseEntity.StartTime).Ticks,
                Comments = exerciseEntity.Comments
            };

            await _ExerciseDbContext.AddAsync(exercise);
            await _ExerciseDbContext.SaveChangesAsync();

            return exercise;
        }
        catch (Exception ex)
        {
            throw new Exception($"{nameof(exerciseEntity)} couldn't be saved: {ex.Message}");
        }
    }

    public async Task<Exercise> UpdateAsync(Exercise exerciseEntity)
    {
        ArgumentNullException.ThrowIfNull(exerciseEntity);

        try
        {
            _ExerciseDbContext.Update(exerciseEntity);
            await _ExerciseDbContext.SaveChangesAsync();

            return exerciseEntity;
        }
        catch (Exception ex)
        {
            throw new Exception($"{nameof(exerciseEntity)} couldn't be updated: {ex.Message}");
        }
    }

    public async Task DeleteAsync(Exercise exerciseEntity)
    {
        ArgumentNullException.ThrowIfNull(exerciseEntity);

        try
        {
            _ExerciseDbContext.Remove(exerciseEntity);
            await _ExerciseDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"{nameof(exerciseEntity)} couldn't be deleted: {ex.Message}");
        }
    }
}
