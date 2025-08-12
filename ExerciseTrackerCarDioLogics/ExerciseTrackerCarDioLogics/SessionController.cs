using ExerciseTrackerCarDioLogics.Models;

namespace ExerciseTrackerCarDioLogics;

public class SessionController
{
    private readonly SessionService _SessionService;

    public SessionController(SessionService SessionService)
    {
        _SessionService = SessionService;
    }

    public void CreateDatabase()
    {
        _SessionService.CreateDatabase();
    }

    public void AddSession(DateTime dateStart, DateTime dateEnd, TimeSpan duration, string comment)
    {
        Session newSession = new Session
        {
            DateStart = dateStart,
            DateEnd = dateEnd,
            Duration = duration,
            Comment = comment
        };

        _SessionService.AddSession(newSession);
    }

    public Session GetSessionById(int id)
    {
        return _SessionService.GetSessionById(id);
    }

    public List<Session> GetAllSessions()
    {
        return _SessionService.GetAllSessions();
    }
    public void UpdateSession(int id, DateTime dateStart, DateTime dateEnd, TimeSpan duration, string comment, Session session)
    {

        session.Id = id;
        session.DateStart = dateStart;
        session.DateEnd = dateEnd;
        session.Duration = duration;
        session.Comment = comment;

        _SessionService.UpdateSession(session);
    }

    public void RemoveSession(int id)
    {
        _SessionService.RemoveSession(id);
    }
}
