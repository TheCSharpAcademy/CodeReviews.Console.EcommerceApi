using ExerciseTracker.Models;

namespace ExerciseTracker.Interfaces;

public interface IExerciseRepository
{
    public void PostExercise(Exercise exercise);

    public List<Exercise> GetAllExercises();

    public Exercise GetExercise(int id);

    public void UpdateExercise(Exercise exercise);

    public void DeleteExercise(Exercise exercise);
}
