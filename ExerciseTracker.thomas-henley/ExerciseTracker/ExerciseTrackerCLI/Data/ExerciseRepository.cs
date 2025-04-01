using ExerciseTrackerCLI.Models;

namespace ExerciseTrackerCLI.Data;

public class ExerciseRepository(ExerciseDbContext context) : IExerciseRepository<TreadmillRun>
{
    /// <summary>
    /// Fetches all stored run data from the database.
    /// </summary>
    /// <returns>A List of all TreadmillRuns.</returns>
    /// <exception cref="DatabaseException">If fetching from the database throws an exception.</exception>
    public IEnumerable<TreadmillRun> GetAll()
    {
        try
        {
            return context.TreadmillRuns.ToList();
        }
        catch (Exception e)
        {
            throw new DatabaseException("There was an error fetching from the database.", e);
        }
    }

    /// <summary>
    /// Fetches a run from the server with the given ID.
    /// </summary>
    /// <param name="id">The primary key ID of the run to fetch.</param>
    /// <returns>A TreadmillRun if the ID exists, otherwise null.</returns>
    /// <exception cref="DatabaseException">If fetching from the database throws an exception.</exception>
    public TreadmillRun? GetById(int id)
    {
        try
        {
            return context.TreadmillRuns.Find(id);
        }
        catch (Exception e)
        {
            throw new DatabaseException("There was an error fetching from the database.", e);
        }
    }

    /// <summary>
    /// Adds a new run to the database.
    /// </summary>
    /// <param name="run">The TreadmillRun to add to the database.</param>
    /// <exception cref="DatabaseException">If saving to the database throws an exception.</exception>
    public void Add(TreadmillRun run)
    {
        context.TreadmillRuns.Add(run);
        try
        {
            context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new DatabaseException("Could not save to database.", e);
        }
    }

    /// <summary>
    /// Updates an existing TreadmillRun on the database.
    /// </summary>
    /// <param name="run">The TreadmillRun to update in the database.</param>
    /// <exception cref="DatabaseException">If saving to the database throws an exception.</exception>
    public void Update(TreadmillRun run)
    {
        context.Update(run);
        try
        {
            context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new DatabaseException("Could not save to database.", e);
        }
    }

    /// <summary>
    /// Deletes an existing TreadmillRun on the database.
    /// </summary>
    /// <param name="run">The TreadmillRun to delete.</param>
    /// <exception cref="DatabaseException">If saving to the database throws an exception.</exception>
    public void Delete(TreadmillRun run)
    {
        context.TreadmillRuns.Remove(run);
        try
        {
            context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new DatabaseException("Could not delete to database.", e);
        }
    }
}