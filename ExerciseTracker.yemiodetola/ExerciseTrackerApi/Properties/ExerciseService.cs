using ExerciseTrackerApi.Repository;
using ExerciseTrackerApi.Models;
using ExerciseTrackerApi.Interfaces;


namespace ExerciseTrackerApi.Properties;

public class ExerciseService
{
  private readonly IExerciseRepository _exerciseRepository;
  public ExerciseService(IExerciseRepository exerciseRepository)
  {
    _exerciseRepository = exerciseRepository;
  }

  public async Task<IEnumerable<Exercise>> GetAllExercisesAsync()
  {
    return await _exerciseRepository.GetAllExercisesAsync();
  }

  public async Task<Exercise> GetExerciseByIdAsync(int id)
  {
    return await _exerciseRepository.GetExerciseByIdAsync(id);
  }

  public async Task AddExerciseAsync(Exercise exercise)
  {
    if (exercise == null)
    {
      throw new ArgumentNullException(nameof(exercise), "Exercise cannot be null.");
    }
    await _exerciseRepository.AddExerciseAsync(exercise);
  }

  public async Task UpdateExerciseAsync(Exercise exercise)
  {
    if (exercise == null)
    {
      throw new ArgumentNullException(nameof(exercise), "Exercise cannot be null.");
    }
    await _exerciseRepository.UpdateExerciseAsync(exercise);
  }

  public async Task DeleteExerciseAsync(int id)
  {
    var existingExercise = await _exerciseRepository.GetExerciseByIdAsync(id);
    if (existingExercise == null)
    {
      throw new KeyNotFoundException($"Exercise with ID {id} not found.");
    }
    await _exerciseRepository.DeleteExerciseAsync(id);
  }

}
