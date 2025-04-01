using ExerciseTrackerCLI.Data;
using ExerciseTrackerCLI.Models;

namespace ExerciseTrackerCLI.Services;

public class ExerciseService(
    IExerciseRepository<TreadmillRun> repository,
    Notifications notifications
    ) : IExerciseService
{
    /// <summary>
    /// Retrieves the TreadmillRun records from the data repository.
    /// </summary>
    /// <param name="runs">Out parameter List that gets populated with the TreadmillRuns from the database.</param>
    /// <returns>True if the repository fetch is successful, false otherwise.</returns>
    public bool TryGetAll(out List<TreadmillRun> runs)
    {
        try
        {
            runs = repository.GetAll().ToList();
        }
        catch (DatabaseException e)
        {
            notifications.AddError("Failed to fetch from the database. Try again later.");
            runs = [];
            return false;
        }

        return true;
    }

    /// <summary>
    /// Adds a TreadmillRun to the data repository.
    /// </summary>
    /// <param name="run">The TreadmillRun to add to the records.</param>
    /// <returns>True if the insert is successful, false otherwise.</returns>
    public bool Add(TreadmillRun run)
    {
        try
        {
            repository.Add(run);
        }
        catch (DatabaseException)
        {
            notifications.AddError("Failed to add run to database. Try again later.");
            return false;
        }
        
        notifications.AddSuccess($"Added {run.Duration.TotalHours:F2} hour run.");
        return true;
    }

    /// <summary>
    /// Updates a TreadmillRun on the data repository.
    /// </summary>
    /// <param name="run">The TreadmillRun entity to update.</param>
    /// <returns>True if the update is successful, false otherwise.</returns>
    public bool Update(TreadmillRun run)
    {
        try
        {
            repository.Update(run);
        }
        catch (DatabaseException)
        {
            notifications.AddError("Failed to update record on database. Try again later.");
            return false;
        }
        
        notifications.AddSuccess($"Successfully updated run.");
        return true;
    }

    /// <summary>
    /// Deletes a TreadmillRun on the data repository.
    /// </summary>
    /// <param name="run">The TreadmillRun entity to delete.</param>
    /// <returns>True if the deletion is successful, false otherwise.</returns>
    public bool Delete(TreadmillRun run)
    {
        try
        {
            repository.Delete(run);
        }
        catch (DatabaseException)
        {
            notifications.AddError("Failed to delete record on database. Try again later.");
            return false;
        }
        
        notifications.AddSuccess($"Successfully deleted run.");
        return true;
    }
}