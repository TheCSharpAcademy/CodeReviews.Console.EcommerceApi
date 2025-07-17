using Exercise_Tracker.Data;
using Exercise_Tracker.Models;

namespace Exercise_Tracker.Repository;

public class ExerciseRepository : IExerciseRepository<Exercise>
{
    private readonly ExerciseDbContext _context;

    public ExerciseRepository(ExerciseDbContext context)
    {
        _context = context;
    }

    public List<Exercise> GetAllExercises()
    {
        return _context.Exercises.ToList();
    }

    public Exercise GetExerciseById(int id)
    {
        return _context.Exercises.Find(id);
    }

    public void AddExercise(Exercise exercise)
    {
        _context.Exercises.Add(exercise);
        _context.SaveChanges();
    }

    public void UpdateExercise(Exercise exercise)
    {
        _context.Exercises.Update(exercise);
        _context.SaveChanges();
    }

    public void DeleteExercise(int id)
    {
        var exercise = GetExerciseById(id);
        if (exercise != null)
        {
            _context.Exercises.Remove(exercise);
            _context.SaveChanges();
        }
        else
        {
            throw new KeyNotFoundException($"Exercise with ID {id} not found.");
        }
    }
}
