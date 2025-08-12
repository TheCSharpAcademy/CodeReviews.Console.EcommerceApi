using ExerciseTracker.Data;
using ExerciseTracker.Models;

namespace ExerciseTracker.Repositories;

public abstract class Repository : IRepository
{
    private readonly ExerciseContext _exerciseContext;

    public Repository(ExerciseContext exerciseContext)
    {
        _exerciseContext = exerciseContext;
    }

    public virtual IEnumerable<Exercise> GetExercises()
    {
        return _exerciseContext.Exercises;
    }

    public virtual Exercise? GetExerciseById(int id)
    {
        return _exerciseContext.Exercises.Find(id);
    }

    public virtual void AddExercise(Exercise exercise)
    {
        _exerciseContext.Add(exercise);
    }

    public virtual bool UpdateExercise(int id, Exercise exercise)
    {
        var exerciseToBeUpdated = GetExerciseById(id);

        exerciseToBeUpdated!.EndDate = exercise.EndDate;
        exerciseToBeUpdated.Duration = exercise.Duration;
        exerciseToBeUpdated.Comments = exercise.Comments;

        return true;
    }

    public virtual bool DeleteExercise(int id)
    {
        var exerciseToBeDeleted = GetExerciseById(id);
        
        if (exerciseToBeDeleted is null)
        {
            Console.Clear();
            Console.WriteLine("Entry does not exist!");
            Console.Write("Press Enter to continue...");
            Console.ReadLine();
            return false;
        }

        _exerciseContext.Remove(exerciseToBeDeleted);
        return true;
    }

    public virtual int SaveChanges()
    {
        return _exerciseContext.SaveChanges();
    }
}