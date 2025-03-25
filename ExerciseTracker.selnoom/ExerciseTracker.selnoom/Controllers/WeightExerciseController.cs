using ExerciseTracker.selnoom.Models;
using ExerciseTracker.selnoom.Services;

namespace ExerciseTracker.selnoom.Controllers;

public class WeightExerciseController
{
    private readonly WeightExerciseService _weightExerciseService;

    public WeightExerciseController(WeightExerciseService weightExerciseService)
    {
        _weightExerciseService = weightExerciseService;
    }

    public async Task<WeightExercise?> GetExerciseByIdAsync(int id)
    {
        return await _weightExerciseService.GetByIdAsync(id);
    }

    public async Task<List<WeightExercise>> GetExercisesAsync()
    {
        return await _weightExerciseService.GetExercisesAsync();
    }

    public async Task<WeightExercise?> CreateExerciseAsync(WeightExercise exercise)
    {
        return await _weightExerciseService.CreateExerciseAsync(exercise);
    }

    public async Task<WeightExercise?> UpdateExerciseAsync(WeightExercise exercise)
    {
        return await _weightExerciseService.UpdateExerciseAsync(exercise);
    }

    public async Task<string?> DeleteExerciseAsync(int id)
    {
        return await _weightExerciseService.DeleteExerciseAsync(id);
    }
}
