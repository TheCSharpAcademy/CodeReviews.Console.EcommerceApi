using ExerciseTracker.Data;
using ExerciseTracker.Models;

namespace ExerciseTracker.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly ExerciseTrackerContext _context;

    public ExerciseRepository(ExerciseTrackerContext context)
    {
        _context = context;
    }

    public void Delete(Exercise exercise)
    {
        _context.Remove(exercise);
    }

    public IEnumerable<Exercise> GetAll()
    {
        return _context.Exercises.ToList();
    }

    public Exercise GetById(int id)
    {
        return _context.Exercises.Find(id);
    }

    public Exercise Insert(Exercise exercise)
    {
        _context.Add(exercise);
        return exercise;
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public Exercise Update(Exercise exercise)
    {
        _context.Update(exercise);
        return exercise;
    }
}
