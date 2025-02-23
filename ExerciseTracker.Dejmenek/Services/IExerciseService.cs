using ExerciseTracker.Dejmenek.Models;

namespace ExerciseTracker.Dejmenek.Services;
public interface IExerciseService
{
    void AddExercise();
    void DeleteExercise();
    void UpdateExercise();
    List<ExerciseReadDTO> GetExercises();
    string CalculateDuration(DateTime startTime, DateTime endTime);
}
