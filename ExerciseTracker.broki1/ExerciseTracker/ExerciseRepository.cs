using ExerciseTracker.Data;
using ExerciseTracker.Interfaces;
using ExerciseTracker.Models;

namespace ExerciseTracker;

public class ExerciseRepository : IExerciseRepository
{
    private readonly ExerciseContext _context;

    public ExerciseRepository(ExerciseContext context)
    {
        _context = context;
    }

    public void PostExercise(Exercise exercise)
    {
        this._context.Exercises.Add(exercise);
        this._context.SaveChanges();
    }

    public List<Exercise> GetAllExercises()
    {
        return this._context.Exercises.ToList();
    }

    public Exercise? GetExercise(int id)
    {
        return this._context.Exercises.FirstOrDefault(x => x.Id == id);
    }

    public void UpdateExercise(Exercise exercise)
    {
        this._context.Exercises.Update(exercise);
        this._context.SaveChanges();
    }

    public void DeleteExercise(Exercise exercise)
    {
        this._context.Exercises.Remove(exercise);
        this._context.SaveChanges();
    }
}
