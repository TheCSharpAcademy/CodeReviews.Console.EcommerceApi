using ExerciseTracker.Models;
using ExerciseTracker.Repositories;

namespace ExerciseTracker.Controllers;

public class RunController : IRunController
{
    private readonly IRunRepository _runRepository;

    public RunController(IRunRepository runRepository)
    {
        _runRepository = runRepository;
    }

    public async Task<Run> GetRunByIdAsync(int id)
    {
        return await _runRepository.GetRunByIdAsync(id);
    }

    public async Task<Run> CreateRunAsync(Run run)
    {
        return await _runRepository.AddRunAsync(run);
    }

    public async Task<List<Run>> GetAllRunsAsync()
    {
        return await _runRepository.GetAllRunsAsync();
    }

    public async Task<Run> UpdateRunAsync(Run run)
    {
        return await _runRepository.UpdateRunAsync(run);
    }

    public async Task<Run> DeleteRunAsync(Run run)
    {
        return await _runRepository.DeleteRunAsync(run);
    }
}