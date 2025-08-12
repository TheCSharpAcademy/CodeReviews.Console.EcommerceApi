using ExerciseTracker.hasona23.Models;

namespace ExerciseTracker.hasona23.Services;

public interface IExerciseService
{
    List<Exercise> GetAllExercises();
    Exercise? GetExercise(int id);
    void AddExercise(ExerciseCreate exercise);
    bool UpdateExercise(ExerciseUpdate newExercise);
    bool DeleteExercise(int id);
}