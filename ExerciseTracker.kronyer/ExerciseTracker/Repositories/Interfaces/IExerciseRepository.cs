using ExerciseTracker.Models;

namespace ExerciseTracker.Repositories.Interfaces;

internal interface IExerciseRepository
{
    void AddExercise(Exercise exercise);
    Exercise GetById(int id);
    List<Exercise> GetAll();
    void UpdateExercise(Exercise exercise);
    void DeleteExercise(int id);
}
