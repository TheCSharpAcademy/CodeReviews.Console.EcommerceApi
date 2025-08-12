using ExerciseTracker.StevieTV.Models;

namespace ExerciseTracker.StevieTV.Repositories;

public interface IExerciseRepository
{
    List<Exercise> GetExercises();
    bool AddExercise(Exercise exercise);
    bool RemoveExercise(Exercise exercise);
}