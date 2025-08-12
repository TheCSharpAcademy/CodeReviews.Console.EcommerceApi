namespace edvaudin.ExerciseTracker.Services;

internal interface IExerciseService
{
    void AddExercise(DateTime start, DateTime end, string? comments);
    void DeleteExercise(int id);
    void UpdateExercise(int id, DateTime start, DateTime end, string? comments);
    void ViewExercises();
}