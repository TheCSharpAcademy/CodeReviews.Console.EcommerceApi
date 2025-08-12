using ExerciseTracker.tonyissa.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.tonyissa.Repositories;

public class ExerciseRepository<T> : IExerciseRepository<T> where T : class, new()
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public ExerciseRepository(ExerciseContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public IEnumerable<T> GetAllSessions()
    {
        return _dbSet.ToList<T>();
    }

    public void AddSession(T session)
    {
        _dbSet.Add(session);
        _context.SaveChanges();
    }

    public void ModifySession(T session)
    {
        _dbSet.Update(session);
        _context.SaveChanges();
    }

    public void DeleteSession(T session)
    {
        _dbSet.Remove(session);
        _context.SaveChanges();
    }
}