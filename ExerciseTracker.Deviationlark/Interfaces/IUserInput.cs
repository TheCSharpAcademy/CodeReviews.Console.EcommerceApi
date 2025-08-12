namespace ExerciseTracker;

public interface IUserInput
{
    Exercise GetExerciseInfo();
    int GetExerciseId(string message, IEnumerable<Exercise> exercises);
}