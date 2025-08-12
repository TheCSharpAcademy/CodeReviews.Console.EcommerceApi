namespace ExerciseTracker.Services;

public interface IExerciseService
{
    public void RecordNewExercise();
    public void UpdateExistingExercise();
    public void GetAllExercises();
    public void GetExerciseById();
    public void DeleteExercise();
}
