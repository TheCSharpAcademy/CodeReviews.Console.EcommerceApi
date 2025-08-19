using ExerciseTracker.selnoom.Data;
using ExerciseTracker.selnoom.Models;

namespace ExerciseTracker.selnoom.Services;

public class WeightExerciseService
{
    private readonly WeightExerciseRepository _weightExerciseRepository;

    public WeightExerciseService(WeightExerciseRepository weightExerciseRepository)
    {
        _weightExerciseRepository = weightExerciseRepository;
    }

    public async Task<WeightExercise?> GetByIdAsync(int id)
    {
        return await _weightExerciseRepository.GetExerciseByIdAsync(id);
    }

    public async Task<List<WeightExercise>> GetExercisesAsync()
    {
        return await _weightExerciseRepository.GetExercisesAsync();
    }

    public async Task<WeightExercise?> CreateExerciseAsync(WeightExercise exercise)
    {
        if (exercise.DateEnd < exercise.DateStart)
        {
            throw new ArgumentException("Start date must be before the end date.");
        }

        if (exercise.Weight <= 0)
        {
            throw new ArgumentException("Weight must be greater than 0.");
        }

        if (exercise.Sets <= 0)
        {
            throw new ArgumentException("Sets must be greater than 0.");
        }

        if (exercise.Repetitions <= 0)
        {
            throw new ArgumentException("Repetitions must be greater than 0.");
        }

        return await _weightExerciseRepository.CreateExerciseAsync(exercise);
    }

    public async Task<WeightExercise?> UpdateExerciseAsync(WeightExercise exercise)
    {
        if (exercise.DateEnd < exercise.DateStart)
        {
            throw new ArgumentException("Start date must be before the end date.");
        }

        if (exercise.Weight <= 0)
        {
            throw new ArgumentException("Weight must be greater than 0.");
        }

        if (exercise.Sets <= 0)
        {
            throw new ArgumentException("Sets must be greater than 0.");
        }

        if (exercise.Repetitions <= 0)
        {
            throw new ArgumentException("Repetitions must be greater than 0.");
        }

        return await _weightExerciseRepository.UpdateExerciseAsync(exercise);
    }

    public async Task<string?> DeleteExerciseAsync(int id)
    {
        return await _weightExerciseRepository.DeleteExerciseAsync(id);
    }
}
