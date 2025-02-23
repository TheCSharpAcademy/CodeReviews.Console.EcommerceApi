namespace ExerciseTracker;
public interface IExerciseRepository
{
    Exercise GetExerciseById(int id);
    IEnumerable<Exercise> GetExercises();
    void AddExercise(Exercise exercise);
    void UpdateExercise(Exercise exercise);
    void DeleteExercise(int id);
    void Save();
}