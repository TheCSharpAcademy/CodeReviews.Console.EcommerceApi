using System.Data.Common;
using ExerciseTracker.StevieTV.Database;
using ExerciseTracker.StevieTV.Models;

namespace ExerciseTracker.StevieTV.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly ExerciseContext _exerciseContext;

    public ExerciseRepository(ExerciseContext exerciseContext)
    {
        _exerciseContext = exerciseContext;
    }

    public List<Exercise> GetExercises()
    {
        return _exerciseContext.Exercises.ToList();
    }

    public bool AddExercise(Exercise exercise)
    {
        _exerciseContext.Exercises.Add(exercise);

        try
        {
            _exerciseContext.SaveChanges();
        }
        catch (DbException)
        {
            return false;
        }

        return true;
    }

    public bool RemoveExercise(Exercise exercise)
    {
        _exerciseContext.Exercises.Remove(exercise);

        try
        {
            _exerciseContext.SaveChanges();
        }
        catch (DbException)
        {
            return false;
        }

        return true;
    }
}