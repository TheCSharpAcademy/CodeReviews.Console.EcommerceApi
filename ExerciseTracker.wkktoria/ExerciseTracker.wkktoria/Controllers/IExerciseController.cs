using ExerciseTracker.wkktoria.Data.Models;

namespace ExerciseTracker.wkktoria.Controllers;

public interface IExerciseController
{
    List<Exercise> GetAllExercises();

    Exercise? GetExercise(int id);

    Exercise? AddExercise(Exercise exercise);

    Exercise UpdateExercise(Exercise exercise);

    void DeleteExercise(int id);
}