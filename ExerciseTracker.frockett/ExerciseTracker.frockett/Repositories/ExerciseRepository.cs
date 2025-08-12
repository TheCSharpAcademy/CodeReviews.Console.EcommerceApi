using ExerciseTracker.frockett.Data;
using ExerciseTracker.frockett.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.frockett.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly ExerciseTrackerContext context;

    public ExerciseRepository(ExerciseTrackerContext context)
    {
        this.context = context;
    }

    public List<ExerciseSession> GetExerciseSessions()
    {
        return context.Sessions.OrderBy(s => s.StartTime).ToList();
    }
    public ExerciseSession? UpdateExerciseSession(ExerciseSession session)
    {
        var sessionToUpdate = context.Sessions.FirstOrDefault(s => s.Id == session.Id);

        if (sessionToUpdate != null)
        {
            sessionToUpdate.StartTime = session.StartTime;
            sessionToUpdate.EndTime = session.EndTime;
            sessionToUpdate.Comments = session.Comments;
            
            try
            {
                context.SaveChanges();
                return sessionToUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        return null;
    }
    public ExerciseSession? GetSessionById(int id)
    {
        return context.Sessions.FirstOrDefault(x => x.Id == id);
    }
    public bool AddExerciseSession(ExerciseSession session)
    {
        context.Sessions.Add(session);
        try
        {
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
        return true;
    }
    public bool RemoveExerciseSession(ExerciseSession session)
    {
        context.Sessions.Remove(session);
        try
        {
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
        return true;
    }
}
