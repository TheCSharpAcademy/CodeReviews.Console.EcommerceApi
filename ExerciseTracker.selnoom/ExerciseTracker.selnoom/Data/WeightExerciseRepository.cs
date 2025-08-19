using ExerciseTracker.Data;
using ExerciseTracker.selnoom.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.selnoom.Data;

public class WeightExerciseRepository
{
    private readonly ExerciseDbContext _exerciseDbContext;

    public WeightExerciseRepository(ExerciseDbContext exerciseDbContext)
    {
        _exerciseDbContext = exerciseDbContext;
    }

    public async Task<WeightExercise?> GetExerciseByIdAsync(int id)
    {
        return await _exerciseDbContext.Exercises.FindAsync(id);
    }

    public async Task<List<WeightExercise>> GetExercisesAsync()
    {
        return await _exerciseDbContext.Exercises.ToListAsync();
    }

    public async Task<WeightExercise> CreateExerciseAsync(WeightExercise exercise)
    {
        var createdExercise = await _exerciseDbContext.Exercises.AddAsync(exercise);
        await _exerciseDbContext.SaveChangesAsync();
        return createdExercise.Entity;
    }

    public async Task<WeightExercise?> UpdateExerciseAsync(WeightExercise exercise)
    {
        WeightExercise? savedExercise = await _exerciseDbContext.Exercises.FindAsync(exercise.Id);

        if (savedExercise == null) return null;

        _exerciseDbContext.Entry(savedExercise).CurrentValues.SetValues(exercise);
        await _exerciseDbContext.SaveChangesAsync();

        return savedExercise;
    }

    public async Task<string?> DeleteExerciseAsync(int id)
    {
        WeightExercise? savedExercise = await _exerciseDbContext.Exercises.FindAsync(id);

        if (savedExercise == null) return null;

        _exerciseDbContext.Exercises.Remove(savedExercise);
        await _exerciseDbContext.SaveChangesAsync();

        return $"Exercise with Id {id} deleted successfully.";
    }
}