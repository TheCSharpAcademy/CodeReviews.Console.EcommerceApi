using ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Repository;

public class ExerciseRepository(ExerciseContext context) : IRepository
{
    private readonly ExerciseContext _context = context;

    public async Task<List<Exercise>> GetAllExercisesAsync()
    {
        return await _context.Exercises.ToListAsync();
    }

    public async Task<Exercise?> GetExerciseByIdAsync(int id)
    {
        return await _context.Exercises.FindAsync(id);
    }

    public async Task AddExerciseAsync(Exercise exercise)
    {
        await _context.Exercises.AddAsync(exercise);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteExerciseByIdAsync(int id)
    {
        Exercise? exercise = await _context.Exercises.FindAsync(id) ?? throw new ApplicationException("No exercise with the given ID exists.");
        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateExerciseAsync(Exercise updatedExercise)
    {
        Exercise? exercise = await _context.Exercises.FindAsync(updatedExercise.Id) ?? throw new ApplicationException("No exercise with the given ID exists.");

        exercise.DateStart = updatedExercise.DateStart;
        exercise.DateEnd = updatedExercise.DateEnd;
        exercise.Comments = updatedExercise.Comments;
        exercise.Type = updatedExercise.Type;

        await _context.SaveChangesAsync();
    }
}