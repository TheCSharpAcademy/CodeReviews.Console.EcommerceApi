
using ExerciseTracker.frockett.Models;

namespace ExerciseTracker.frockett.Repositories;

public interface IExerciseRepository
{
    List<ExerciseSession>? GetExerciseSessions();
    ExerciseSession? UpdateExerciseSession(ExerciseSession session);
    ExerciseSession? GetSessionById(int id);
    bool AddExerciseSession(ExerciseSession session);
    bool RemoveExerciseSession(ExerciseSession session);
}
