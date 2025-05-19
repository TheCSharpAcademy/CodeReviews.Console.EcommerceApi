using ExerciseTracker.KamilKolanowski.Models;
using ExerciseTracker.KamilKolanowski.Models.Data;
using Spectre.Console;

namespace ExerciseTracker.KamilKolanowski.Repositories;

internal class ExerciseRepository : IExerciseRepository
{
    private readonly ExerciseTrackerDbContext _context;

    public ExerciseRepository(ExerciseTrackerDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Exercise?> GetExercises()
    {
        var allExercises = _context.Exercises;
        return allExercises;
    }

    public Exercise? GetExercise(int id)
    {
        return _context.Exercises.Find(id);
    }

    public void Insert(Exercise exercise)
    {
        try
        {
            _context.Exercises.Add(exercise);
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
        }
        Save();
    }

    public void Update(Exercise exerciseObject)
    {
        try
        {
            var exercise = _context.Exercises.Find(exerciseObject.Id);

            if (exercise != null)
            {
                exercise.DateStart = exerciseObject.DateStart;
                exercise.DateEnd = exerciseObject.DateEnd;
                exercise.Comment = exerciseObject.Comment;
            }

            _context.Exercises.Update(exerciseObject);
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
        }
        Save();
    }

    public void Delete(int id)
    {
        try
        {
            var exercise = _context.Exercises.Find(id);
            if (exercise != null)
            {
                _context.Exercises.Remove(exercise);
            }
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
        }
        
        Save();
    }

    public void Save()
    {
        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
        }
    }
}
