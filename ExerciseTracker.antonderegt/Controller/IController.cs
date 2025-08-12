using ExerciseTracker.Models;

namespace ExerciseTracker.Controller;

public interface IController
{
    Task MainMenu();
    Task<List<Exercise>?> ShowAllExercisesAsync();
    Task AddExerciseAsync();
    Task UpdateExerciseAsync();
    Task DeleteExerciseByIdAsync();
}