using ExerciseTracker.Arashi256.Models;
using ExerciseTracker.Arashi256.Classes;

namespace ExerciseTracker.Arashi256.Interfaces
{
    public interface IExerciseSessionRepository
    {
        ServiceResponse GetExerciseSessions();
        ServiceResponse GetExerciseSessionById(int id);
        ServiceResponse AddExerciseSession(ExerciseSession exercise);
        ServiceResponse DeleteExerciseSession(int id);
        ServiceResponse UpdateExerciseSession(int id, ExerciseSession exercise);
        ServiceResponse ExerciseSessionExistsInRange(DateTime startDate, DateTime endDate, int? sessionIdToExclude = null);
    }
}