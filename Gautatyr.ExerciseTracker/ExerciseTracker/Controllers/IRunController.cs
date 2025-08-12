using ExerciseTracker.Models;

namespace ExerciseTracker.Controllers;

public interface IRunController
{
    Task<List<Run>> GetAllRunsAsync();

    Task<Run> GetRunByIdAsync(int id);

    Task<Run> CreateRunAsync(Run run);

    Task<Run> UpdateRunAsync(Run run);

    Task<Run> DeleteRunAsync(Run run);
}