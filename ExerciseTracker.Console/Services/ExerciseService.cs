using ExerciseTracker.Niasua.Models;
using ExerciseTracker.Niasua.Repositories;

namespace ExerciseTracker.Niasua.Services;

public class ExerciseService
{
    private readonly IExerciseRepository _repository;

    public ExerciseService(IExerciseRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> CreateExerciseAsync(Exercise exercise)
    {
        if (!Validators.ExerciseValidator.IsValid(exercise))
        {
            return false;
        }

        await _repository.AddAsync(exercise);
        return true;
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

    public async Task<bool> UpdateExercisesAsync(Exercise exercise)
    {
        if (!Validators.ExerciseValidator.IsValid(exercise))
        {
            return false;
        }

        await _repository.UpdateAsync(exercise);
        return true;
    }
}
