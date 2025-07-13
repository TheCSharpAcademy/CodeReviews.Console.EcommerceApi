using ExerciseTrackerApi.Models;
namespace ExerciseTrackerApi.Interfaces;

public interface IExerciseRepository
{
  Task<IEnumerable<Exercise>> GetAllExercisesAsync();
  Task<Exercise> GetExerciseByIdAsync(int id);
  Task AddExerciseAsync(Exercise exercise);
  Task UpdateExerciseAsync(Exercise exercise);
  Task DeleteExerciseAsync(int id);
}
