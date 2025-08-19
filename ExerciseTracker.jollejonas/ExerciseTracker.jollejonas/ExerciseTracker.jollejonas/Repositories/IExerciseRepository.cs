using ExerciseTracker.jollejonas.Models;

namespace ExerciseTracker.jollejonas.Repositories;

public interface IExerciseRepository
{
    List<Exercise> GetAllExercises();
    Exercise GetExerciseById(int id);
    void AddExercise(Exercise exercise);
    void UpdateExercise(Exercise exercise);
    void DeleteExercise(Exercise exercise);
}
