using ExerciseTracker.frockett.Models;
using ExerciseTracker.frockett.Services;

namespace ExerciseTracker.frockett.Controllers;

public class ExerciseController
{
    private readonly IExerciseService exerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        this.exerciseService = exerciseService;
    }

    public bool AddExerciseSession(ExerciseSession session)
    {
        return exerciseService.AddExerciseSession(session);
    }

    public List<ExerciseSession>? GetExerciseSessions()
    {
        return exerciseService.GetExerciseSessions();
    }

    public ExerciseSession? UpdateExerciseSession(ExerciseSession session)
    {
        return exerciseService.UpdateExerciseSession(session);
    }

    public bool RemoveExerciseSession(ExerciseSession session)
    {
        return exerciseService.RemoveExerciseSession(session);
    }
}
