using ExerciseTrackerCarDioLogics.Models;

namespace ExerciseTrackerCarDioLogics.Repositories;

public interface ISessionRepository
{
    Session GetSessionById(int id);
    List<Session> GetAllSessions();
    void AddSession(Session session);
    void RemoveSession(int id);
    void UpdateSession(Session session);
    void CreateDatabase();
}
