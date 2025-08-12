using ExerciseTracker.Dejmenek.Models;

namespace ExerciseTracker.Dejmenek.Data.Interfaces;
public interface IExerciseRepository
{
    void AddExercise(Exercise exercise);
    void DeleteExercise(int exerciseId);
    void UpdateExercise(int exerciseId, ExerciseUpdateDTO exerciseDto);
    Exercise GetExerciseById(int exerciseId);
    List<Exercise> GetExercises();
}
