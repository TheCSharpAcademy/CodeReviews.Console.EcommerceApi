using ExerciseTracker.Models;

namespace ExerciseTracker.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
{
    protected readonly ExerciseTrackerContext ExerciseTrackerContext;

    public Repository(ExerciseTrackerContext exerciseTrackerContext)
    {
        ExerciseTrackerContext = exerciseTrackerContext;
    }

    public IQueryable<TEntity> GetAll()
    {
        try
        {
            return ExerciseTrackerContext.Set<TEntity>();
        }
        catch (Exception ex)
        {
            throw new Exception($"\nCouldn't retrieve entities: {ex.Message}");
        }
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
        }

        try
        {
            await ExerciseTrackerContext.AddAsync(entity);
            await ExerciseTrackerContext.SaveChangesAsync();

            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
        }
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(UpdateAsync)} entity must not be null");
        }

        try
        {
            ExerciseTrackerContext.Update(entity);
            await ExerciseTrackerContext.SaveChangesAsync();

            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
        }
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(DeleteAsync)} entity must not be null");
        }

        try
        {
            ExerciseTrackerContext.Remove(entity);
            await ExerciseTrackerContext.SaveChangesAsync();

            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
        }
    }
}