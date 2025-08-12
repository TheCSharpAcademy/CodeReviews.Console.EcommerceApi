using ExerciseTracker.kwm0304.Models;
using ExerciseTracker.kwm0304.Repositories;

namespace ExerciseTracker.kwm0304.Services;

public class ExerciseService
{
  private readonly ExerciseRepository _repository;

  public ExerciseService(ExerciseRepository repository)
  {
    _repository = repository;
  }

  public async Task<List<UserInput>> GetAllExercisesAsync()
  {
    return await _repository.GetAllAsync();
  }

  public async Task<UserInput> GetExerciseByIdAsync(int id)
  {
    return await _repository.GetByIdAsync(id);
  }

  public async Task CreateExerciseAsync(UserInput exercise)
  {
    await _repository.CreateAsync(exercise);
  }

  public async Task UpdateExerciseAsync(UserInput exercise)
  {
    await _repository.UpdateAsync(exercise);
  }

  public async Task DeleteExerciseByIdAsync(int id)
  {
    await _repository.DeleteByIdAsync(id);
  }
}