using ExerciseTrackerCLI.Models;

namespace ExerciseTrackerCLI.Services;

public interface IExerciseService
{
    bool TryGetAll(out List<TreadmillRun> runs);
    bool Add(TreadmillRun run);
    bool Update(TreadmillRun run);
    bool Delete(TreadmillRun run);
}