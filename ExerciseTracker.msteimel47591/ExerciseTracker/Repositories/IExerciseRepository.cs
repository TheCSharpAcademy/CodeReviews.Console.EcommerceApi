using ExerciseTracker.Models;

namespace ExerciseTracker.Repositories;

internal interface IExerciseRepository
{
    public List<Exercise> GetExercises();

    public Exercise GetExercise(int id);

    public void AddExercise(Exercise exercise);

    public void UpdateExercise(int id, Exercise exercise);

    public void DeleteExercise(int id);
}
