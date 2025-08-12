namespace ExerciseTracker;

public interface IExerciseService
{
    void MainMenu();
    void AddExercise();
    void DeleteExercise();
    void GetExerciseById();
    void GetExercises(int num = 0);
    void UpdateExercise();
}