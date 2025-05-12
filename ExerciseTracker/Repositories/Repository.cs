using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Repositories;

class ExerciceRepository<T> : IRepository<T> where T : class
{
    private readonly DbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public ExerciceRepository(DbContext context)
    {
        _dbContext = context;
        _dbSet = context.Set<T>();
    }
    public void Add(T entity)
    {
        if (entity != null)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }
    }

    public void Delete(T entity)
    {
        if (entity != null)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
            string sqliteCommand = @$"WITH dlt as (SELECT Id FROM {typeof(T).Name} WHERE Id+1 NOT IN (SELECT Id FROM {typeof(T).Name}) AND Id+1 NOT IN(SELECT Max(Id) FROM {typeof(T).Name}))
                                      Update {typeof(T).Name} SET Id = CASE WHEN (SELECT count(Id) From {typeof(T).Name}) = 1 THEN 1 ELSE Id END ;
                                      
                                      WITH dlt as (SELECT Id FROM {typeof(T).Name} WHERE Id+1 NOT IN (SELECT Id FROM {typeof(T).Name})) Update {typeof(T).Name} SET Id = Id-1 WHERE Id > (SELECT min(Id) From dlt);
                                      
                                      UPDATE {typeof(T).Name} SET Id = CASE WHEN (SELECT min(Id) FROM {typeof(T).Name}) = 2 THEN Id-1 ELSE Id END;";
            _dbContext.Database.ExecuteSqlRaw(sqliteCommand);
        }
    }

    public List<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public T? GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public void Update(T entity)
    {

        if (entity != null)
        {
            _dbSet.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}