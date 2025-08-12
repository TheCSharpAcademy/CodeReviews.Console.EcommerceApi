using ExerciseTracker.UgniusFalze.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.UgniusFalze.Repositories;

public class ExerciseRepository(PullUpContext pullUpContext) : IExerciseRepository
{
    public List<Pullup> GetExercises()
    {
        return pullUpContext.Pullups.ToList();
    }

    public bool InsertExercise(Pullup pullup)
    {
        pullUpContext.Pullups.Add(pullup);
        try
        {
            pullUpContext.SaveChanges();
        }
        catch(DbUpdateException)
        {
            return false;
        }

        return true;
    }

    public bool UpdateExercise(Pullup pullup)
    {
        pullUpContext.Pullups.Update(pullup);
        try
        {
            pullUpContext.SaveChanges();
        }
        catch(DbUpdateException)
        {
            return false;
        }
        return true;
    }

    public bool DeleteExercise(int id)
    {
        var exercise = pullUpContext.Pullups.Find(id);
        if (exercise == null)
        {
            return false;
        }

        pullUpContext.Pullups.Remove(exercise);
        try
        {
            pullUpContext.SaveChanges();
        }
        catch(DbUpdateException)
        {
            return false;
        }
        return true;
    }
}