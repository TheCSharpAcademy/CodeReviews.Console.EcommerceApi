using ExerciseTracker.frockett.Models;


namespace ExerciseTracker.frockett.Services;

public interface IExerciseService
{
    List<ExerciseSession>? GetExerciseSessions();
    ExerciseSession? UpdateExerciseSession(ExerciseSession session);
    ExerciseSession? GetSessionById(int id);
    bool AddExerciseSession(ExerciseSession session);
    bool RemoveExerciseSession(ExerciseSession session);
}
