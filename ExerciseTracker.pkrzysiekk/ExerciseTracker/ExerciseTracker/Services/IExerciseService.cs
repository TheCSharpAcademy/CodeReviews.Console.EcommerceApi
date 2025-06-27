using ExerciseTracker.Models;

namespace ExerciseTracker.Services;

public interface IExerciseService
{
    public Task AddExercise(Exercise exercise);

    public Task DeleteExercise(Exercise exercise);

    public Task EditExercise(Exercise exercise);

    public Task<IEnumerable<Exercise>> GetAllExercises();
}