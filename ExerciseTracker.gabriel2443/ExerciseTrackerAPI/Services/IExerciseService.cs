using ExerciseTracker.Models;

namespace ExerciseTracker.Services;

public interface IExerciseService
{
    Task<List<Exercise>> GetAllExercises();

    Task<Exercise> GetExerciseById(int id);

    Task CreateExercise(Exercise exercise);

    Task UpdateExercise(int id, Exercise exerciseToUpdate);

    Task DeleteExercise(int id);
}