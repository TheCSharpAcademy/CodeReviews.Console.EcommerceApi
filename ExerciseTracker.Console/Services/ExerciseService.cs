using ExerciseTracker.Console.Models;
using ExerciseTracker.Console.Repositories;

namespace ExerciseTracker.Console.Services;

public class ExerciseService
{
    private readonly ExerciseRepository _repository;

    public ExerciseService(ExerciseRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateExerciseAsync(Exercise exercise)
    {
        await _repository.AddAsync(exercise);
    }

    public async Task DeleteExerciseAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<List<Exercise>> GetAllExercisesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Exercise?> GetExercisesByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task UpdateExercisesAsync(Exercise exercise)
    {
        await _repository.UpdateAsync(exercise);
    }
}
