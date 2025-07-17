using Exercise_Tracker.Models;

namespace Exercise_Tracker.Services;

public interface IExerciseService
{
    List<Exercise> GetAllExercises();

    Exercise GetExerciseById(int id);

    void AddExercise();

    void UpdateExercise(Exercise exercise);

    void DeleteExercise(int id);
}
