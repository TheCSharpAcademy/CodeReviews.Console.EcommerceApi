using ExerciseTrackerCarDioLogics.Models;
using ExerciseTrackerCarDioLogics.Repositories;

namespace ExerciseTrackerCarDioLogics;

public class SessionService
{
    private readonly ISessionRepository _SessionRepo;

    public SessionService(ISessionRepository SessionRepo)
    {
        _SessionRepo = SessionRepo;
    }

    public void CreateDatabase()
    {
        _SessionRepo.CreateDatabase();
    }

    public Session GetSessionById(int id)
    {
        return _SessionRepo.GetSessionById(id);
    }

    public List<Session> GetAllSessions()
    {
        return _SessionRepo.GetAllSessions();
    }

    public void AddSession(Session session)
    {
        _SessionRepo.AddSession(session);
    }

    public void RemoveSession(int id)
    {
        _SessionRepo.RemoveSession(id);
    }

    public void UpdateSession(Session session)
    {
        _SessionRepo.UpdateSession(session);
    }
}
