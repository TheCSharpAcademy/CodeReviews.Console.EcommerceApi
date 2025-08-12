using GymExerciseTracker.Data;
using GymExerciseTracker.Models;

namespace GymExerciseTracker.Repository;
public class ExerciseRepository : IExerciseRepository
{
    private readonly ApplicationDbContext _context;

    public ExerciseRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public GymSession CreateSession(GymSession newSession)
    {
        _context.GymSessions.Add(newSession);
        _context.SaveChanges();

        return newSession;
    }

    public GymSession DeleteSession(int id)
    {
        var session = _context.GymSessions.FirstOrDefault(gs => gs.Id == id);
        if (session == null) return null;

        _context.GymSessions.Remove(session);
        _context.SaveChanges();

        return session;
    }

    public List<GymSession> GetAllSessions()
    {
        return _context.GymSessions.ToList();
    }

    public GymSession GetSession(int id)
    {
        var session = _context.GymSessions.FirstOrDefault(gs => gs.Id == id);
        if (session == null) return null;

        return session;
    }

    public GymSession UpdateSession(int id, GymSession updateSession)
    {
        if (id != updateSession.Id) return null;

        var session = _context.GymSessions.FirstOrDefault(gs => gs.Id == id);
        if (session == null) return null;

        _context.GymSessions.Update(updateSession);
        _context.SaveChanges();

        return updateSession;
    }
}

