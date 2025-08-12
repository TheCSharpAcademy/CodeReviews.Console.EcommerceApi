using ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Repositories;

public class RunRepository : Repository<Run>, IRunRepository
{
    private readonly ExerciseTrackerContext _context;

    public RunRepository(ExerciseTrackerContext exerciseTrackerContext) : base(exerciseTrackerContext)
    {
        _context = exerciseTrackerContext;
    }

    public async Task<Run> GetRunByIdAsync(int id)
    {
        return await _context.Run.FindAsync(id);
    }

    public async Task<List<Run>> GetAllRunsAsync()
    {
        return await _context.Run.ToListAsync();
    }

    public async Task<Run> UpdateRunAsync(Run run)
    {
        return await UpdateAsync(run);
    }

    public async Task<Run> DeleteRunAsync(Run run)
    {
        return await DeleteAsync(run);
    }

    public async Task<Run> AddRunAsync(Run run)
    {
        return await AddAsync(run);
    }
}