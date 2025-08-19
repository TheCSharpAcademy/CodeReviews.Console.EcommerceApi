using ExerciseTracker.jollejonas.Data;
using ExerciseTracker.jollejonas.Models;

namespace ExerciseTracker.jollejonas.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly ExerciseContext _context;

    public ExerciseRepository(ExerciseContext context)
    {
        _context = context;
    }

    public void AddExercise(Exercise exercise)
    {
        _context.Exercises.Add(exercise);
        _context.SaveChanges();
    }

    public void DeleteExercise(Exercise exercise)
    {
        _context.Exercises.Remove(exercise);
        _context.SaveChanges();
    }

    public List<Exercise> GetAllExercises()
    {
        return _context.Exercises.ToList();
    }

    public Exercise GetExerciseById(int id)
    {
        if (id == 0)
        {
            throw new ArgumentNullException();
        }
        if (!_context.Exercises.Any(e => e.Id == id))
        {
            throw new KeyNotFoundException();
        }
        return _context.Exercises.FirstOrDefault(e => e.Id == id);
    }

    public void UpdateExercise(Exercise exercise)
    {
        _context.Exercises.Update(exercise);
        _context.SaveChanges();
    }
}
