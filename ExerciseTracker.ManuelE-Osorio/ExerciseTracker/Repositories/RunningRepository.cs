using ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Repositories;

public class RunningRepository(ExerciseTrackerContext dbContext): IExerciseRepository<Running>
{
    private readonly ExerciseTrackerContext DbContext = dbContext;

    public bool TryConnection()
    {
        try
        {
            DbContext.Database.EnsureCreated();
            DbContext.Database.OpenConnection();
            DbContext.Database.CanConnect();
            return true;
        }
        catch 
        {
            throw new Exception("The app cannot connect to the Database. "+
                "Please check your Connection String configuration in your appsettings.json");
        }
    }

    public bool Insert(Running model)
    {
        DbContext.RunningExercise.Add(model);
        DbContext.SaveChanges();
        return true;
    }

    public IEnumerable<Running>? GetAll()
    {
        return DbContext.RunningExercise.AsEnumerable();
    }
    
    public Running? GetById(int id)
    {
        return DbContext.RunningExercise.Find(id);
    }

    public bool Update(Running model)
    {
        var runningToUpdate = GetById(model.Id);
        if ( runningToUpdate == null)
            return false;

        DbContext.Entry(runningToUpdate).CurrentValues.SetValues(model);
        DbContext.SaveChanges();
        return true;
    }

    public bool Delete(Running model)
    {
        var runningToDelete = DbContext.RunningExercise.Where(p => p.Id == model.Id).First();
        if ( runningToDelete == null)
            return false;

        DbContext.Remove(runningToDelete);
        DbContext.SaveChanges();
        return true;
    }
}