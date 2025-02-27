using ExerciseTracker.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.EF.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public T Add(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();

        return entity;
    }

    public T? Delete(int id)
    {
        var entity = GetById(id);
        if (entity == null)
        {
            return null;
        }

        _dbSet.Remove(entity);
        _context.SaveChanges();

        return entity;
    }

    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public T? GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public T? Update(int id, T entity)
    {
        var existingEntity = _dbSet.Find(id);

        if (existingEntity == null)
        {
            return null;
        }

        foreach (var property in typeof(T).GetProperties())
        {
            if (!property.CanWrite || property.Name == "Id") continue;

            var newValue = property.GetValue(entity);
            if (newValue != null)
            {
                property.SetValue(existingEntity, newValue);
            }
        }

        _context.SaveChanges();
        return existingEntity;
    }
}