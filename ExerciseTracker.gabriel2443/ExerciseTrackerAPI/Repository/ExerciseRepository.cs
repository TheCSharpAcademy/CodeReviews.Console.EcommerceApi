using ExerciseTracker.Data;
using ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTrackerAPI.Repository;

public class ExerciseRepository : IExerciseRepository
{
    private readonly DataContext _context;

    public ExerciseRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Exercise>> GetAllExercises()
    {
        return await _context.Exercises.ToListAsync();
    }

    public async Task<Exercise> GetExerciseById(int id)
    {
        return await _context.Exercises.FindAsync(id);
    }

    public async Task CreateExercise(Exercise exercise)
    {
        _context.Exercises.Add(exercise);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateExercise(int id, Exercise exerciseToUpdate)
    {
        var exerciseDb = await _context.Exercises.FindAsync(id);

        exerciseDb.ExerciseName = exerciseToUpdate.ExerciseName;
        exerciseDb.StartTime = exerciseToUpdate.StartTime;
        exerciseDb.EndTime = exerciseToUpdate.EndTime;
        exerciseDb.Duration = exerciseToUpdate.EndTime - exerciseToUpdate.StartTime;
        exerciseDb.Comments = exerciseToUpdate.Comments;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteExercise(int id)
    {
        var exerciseDb = await _context.Exercises.FindAsync(id);

        if (exerciseDb is not null) _context.Exercises.Remove(exerciseDb);
        await _context.SaveChangesAsync();
    }
}