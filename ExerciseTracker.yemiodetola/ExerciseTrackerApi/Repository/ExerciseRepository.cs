using ExerciseTrackerApi.Models;
using ExerciseTrackerApi.Interfaces;
using ExerciseTrackerApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTrackerApi.Repository;

public class ExerciseRepository : IExerciseRepository
{
  private readonly ExerciseContext _context;

  public ExerciseRepository(ExerciseContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Exercise>> GetAllExercisesAsync()
  {
    return await _context.Exercises.ToListAsync();
  }

  public async Task<Exercise> GetExerciseByIdAsync(int id)
  {
    return await _context.Exercises.FindAsync(id);
  }

  public async Task AddExerciseAsync(Exercise exercise)
  {
    _context.Exercises.Add(exercise);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateExerciseAsync(Exercise exercise)
  {
    _context.Exercises.Update(exercise);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteExerciseAsync(int id)
  {
    var exercise = await _context.Exercises.FindAsync(id);
    if (exercise != null)
    {
      _context.Exercises.Remove(exercise);
      await _context.SaveChangesAsync();
    }
  }
}
