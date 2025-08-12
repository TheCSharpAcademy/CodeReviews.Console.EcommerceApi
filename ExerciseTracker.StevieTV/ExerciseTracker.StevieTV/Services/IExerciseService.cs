using ExerciseTracker.StevieTV.Models;

namespace ExerciseTracker.StevieTV.Services;

public interface IExerciseService
{
    List<Exercise> GetExercises();
    bool AddExercise(Exercise exercise);
    bool RemoveExercise(Exercise exercise);
}