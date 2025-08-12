namespace ExerciseTracker.Repositories;

public interface IRepository<TEntity> where TEntity : class, new()
{
    IQueryable<TEntity> GetAll();

    Task<TEntity> AddAsync(TEntity entity);

    Task<TEntity> UpdateAsync(TEntity entity);
}