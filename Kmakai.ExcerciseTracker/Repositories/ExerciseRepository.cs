

using Kmakai.ExerciseTracker.DataAccess;
using Kmakai.ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Kmakai.ExerciseTracker.Repositories;

public class ExerciseRepository:IRepository<Exercise>, IExerciseRepository
{
    
    private readonly ExerciseContext Context;

    public ExerciseRepository(ExerciseContext context)
    {
        Context = context;
    }

    public async Task<Exercise> GetAsync(int id)
    {
        return await Context.Exercises.FindAsync(id) ?? new Exercise();
    }

    public async Task<IEnumerable<Exercise>> GetAllAsync()
    {
        return await Context.Exercises.ToListAsync();
    }

    public async Task<Exercise> AddAsync(Exercise entity)
    {
        Context.Exercises.Add(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<Exercise> UpdateAsync(Exercise entity)
    {
        Context.Exercises.Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<Exercise> DeleteAsync(int id)
    {
        var entity = await Context.Exercises.FindAsync(id);
        Context.Exercises.Remove(entity!);
        await Context.SaveChangesAsync();
        return entity!;
    }

    public Exercise Get(int id)
    {
        return Context.Exercises.Find(id) ?? new Exercise();
    }

    public IEnumerable<Exercise> GetAll()
    {
        return Context.Exercises.ToList();
    }

    public Exercise Add(Exercise entity)
    {
        Context.Exercises.Add(entity);
        Context.SaveChanges();
        return entity;
    }

    public Exercise Update(Exercise entity)
    {
        Context.Exercises.Update(entity);
        Context.SaveChanges();
        return entity;
    }

    public Exercise Delete(int id)
    {
        var entity = Context.Exercises.Find(id);
        Context.Exercises.Remove(entity!);
        Context.SaveChanges();
        return entity!;
    }       
}
