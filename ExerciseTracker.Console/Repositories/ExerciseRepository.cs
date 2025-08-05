using ExerciseTracker.Niasua.Data;
using ExerciseTracker.Niasua.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Niasua.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly ExerciseContext _context;

    public ExerciseRepository(ExerciseContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Exercise exercise)
    {
        await _context.Exercises.AddAsync(exercise);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var exercise = await GetByIdAsync(id);
        if (exercise != null)
        {
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Exercise>> GetAllAsync()
    {
        return await _context.Exercises.ToListAsync();
    }

    public async Task<Exercise?> GetByIdAsync(int id)
    {
        return await _context.Exercises.FindAsync(id);
    }

    public async Task UpdateAsync(Exercise exercise)
    {
        _context.Exercises.Update(exercise);
        await _context.SaveChangesAsync();
    }
}
