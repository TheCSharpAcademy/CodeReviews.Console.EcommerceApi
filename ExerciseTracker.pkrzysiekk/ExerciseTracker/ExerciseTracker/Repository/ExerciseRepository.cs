using ExerciseTracker.Data;
using ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Repository;

public class ExerciseRepository : IRepository<Exercise>
{
    private readonly ExerciseContext _context;

    public ExerciseRepository(ExerciseContext context)
    {
        _context = context;
    }

    public async Task<Exercise?> GetById(int id)
    {
        var exercise = await _context.Exercises.FindAsync(id);
        return exercise;
    }

    public async Task<IEnumerable<Exercise>> GetAll()
    {
        var exercises = await _context.Exercises.ToListAsync();
        return exercises;
    }

    public async Task Insert(Exercise entity)
    {
        _context.Exercises.Add(entity);
        await SaveChanges();
    }

    public async Task Update(Exercise entity)
    {
        var exercise = await GetById(entity.Id);
        if (exercise != null)
        {
            exercise.Description = entity.Description;
            exercise.DateEnd = entity.DateEnd;
            exercise.DateStart = entity.DateStart;
            await SaveChanges();
        }
    }

    public async Task Delete(Exercise entity)
    {
        var exercise = await GetById(entity.Id);
        if (exercise != null)
        {
            _context.Exercises.Remove(exercise);
            await SaveChanges();
        }
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}