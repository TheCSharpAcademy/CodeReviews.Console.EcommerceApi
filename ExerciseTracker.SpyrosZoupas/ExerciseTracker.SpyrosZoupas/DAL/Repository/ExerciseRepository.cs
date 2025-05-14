using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.SpyrosZoupas.DAL.Repository;

public class ExerciseRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class, new()
{
    private readonly ExerciseTrackerDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public ExerciseRepository(ExerciseTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _dbContext.Set<TEntity>().ToList();
    }
    public TEntity GetById(int id)
    {
        return _dbSet.Find(id);
    }   

    public void Insert(TEntity entity)
    {
        _dbContext.Add(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbContext.Remove(entity);
    }

    public void Update(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Save() =>
        _dbContext.SaveChanges();

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

