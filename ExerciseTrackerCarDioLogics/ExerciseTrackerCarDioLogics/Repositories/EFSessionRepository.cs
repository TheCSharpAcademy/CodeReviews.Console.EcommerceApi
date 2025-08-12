using ExerciseTrackerCarDioLogics.Data;
using ExerciseTrackerCarDioLogics.Models;

namespace ExerciseTrackerCarDioLogics.Repositories;

//implementation of the ExSession interface into Entity Framework
public class EFSessionRepository : ISessionRepository
{
    private readonly SessionContext _context;

    //constructor
    public EFSessionRepository(SessionContext context)
    {
        _context = context;
    }

    public void CreateDatabase()
    {
        _context.Database.EnsureCreated();
    }

    public Session GetSessionById(int id)
    {
        return _context.Sessions.Find(id);
    }

    public List<Session> GetAllSessions()
    {
        return _context.Sessions.ToList();
    }

    public void AddSession(Session session)
    {
        _context.Sessions.Add(session);
        _context.SaveChanges();
    }

    public void RemoveSession(int id)
    {
        var session = _context.Sessions.Find(id);

        if (session != null)
        {
            _context.Sessions.Remove(session);
            _context.SaveChanges();
        }
    }

    public void UpdateSession(Session session)
    {
        _context.Sessions.Update(session);
        _context.SaveChanges();
    }
}
