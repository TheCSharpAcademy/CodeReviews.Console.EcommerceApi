using GymExerciseTracker.Models;

namespace GymExerciseTracker.Repository;
public interface IExerciseRepository
{
    List<GymSession> GetAllSessions();
    GymSession GetSession(int id);
    GymSession CreateSession(GymSession newSession);
    GymSession UpdateSession(int id, GymSession updateSession);
    GymSession DeleteSession(int id);
}

