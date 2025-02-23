
using Microsoft.EntityFrameworkCore;

namespace App.Database.EntityFramework;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class, new()
{
    protected readonly ExercisesDbContext Db;

    public RepositoryBase(ExercisesDbContext db)
    {
        Db = db;
    }

    public async Task<bool?> DeleteOne(int id)
    {
        T existingEntity = await GetById(id);

        Db.Remove(existingEntity);
        await Db.SaveChangesAsync();

        var entityAfterDeletion = await FindById(id);

        return entityAfterDeletion == null;
    }

    public async Task<T?> FindById(int id)
    {
        var result = await Db.FindAsync(typeof(T), id);

        return (T?)result;
    }

    public async Task<T> GetById(int id)
    {
        var result = await Db.FindAsync(typeof(T), id);

        if (result == null)
        {
            throw new Exception($"Could not find {nameof(T)} by ID {id}");
        }

        return (T)result;
    }

    public async Task<List<T>> ListAll()
    {
        return await Db.Set<T>().ToListAsync();
    }

    public async Task<T> CreateOne(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(T)} cannot be null");
        }

        await Db.AddAsync(entity);

        await Db.SaveChangesAsync();

        return entity;
    }

    public async Task<T> UpdateOne(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(T)} cannot be null");
        }

        Db.Update(entity);

        await Db.SaveChangesAsync();

        return entity;
    }
}