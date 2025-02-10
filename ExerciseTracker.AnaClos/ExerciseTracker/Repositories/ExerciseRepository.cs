using ExerciseTracker.Data;
using ExerciseTracker.Models;

namespace ExerciseTracker.Repositories;

public class ExerciseRepository : IRepository
{
    private ExerciseDbContext _context;
    void IRepository.Add(Exercise exercise)
    {
        try
        {
            _context.Exercises.Add(exercise);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    void IRepository.Delete(Exercise exercise)
    {
        try
        {
            _context.Exercises.Remove(exercise);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    IEnumerable<Exercise> IRepository.GetAll()
    {
        try
        {
            return _context.Exercises.ToList();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    Exercise IRepository.GetById(int id)
    {
        try
        {
            return _context.Exercises.Find(id);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    void IRepository.Update(Exercise exercise)
    {
        try
        {
            _context.Exercises.Update(exercise);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
