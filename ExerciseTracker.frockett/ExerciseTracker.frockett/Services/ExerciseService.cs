using ExerciseTracker.frockett.Models;
using ExerciseTracker.frockett.Repositories;

namespace ExerciseTracker.frockett.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository exerciseRepository;

    public ExerciseService(IExerciseRepository exerciseRepository)
    {
        this.exerciseRepository = exerciseRepository;
    }

    public bool AddExerciseSession(ExerciseSession session)
    {
        return exerciseRepository.AddExerciseSession(session);
    }

    public List<ExerciseSession>? GetExerciseSessions()
    {
        return exerciseRepository.GetExerciseSessions();
    }

    public ExerciseSession? GetSessionById(int id)
    {
        return exerciseRepository.GetSessionById(id);
    }

    public bool RemoveExerciseSession(ExerciseSession session)
    {
        return exerciseRepository.RemoveExerciseSession(session);
    }

    public ExerciseSession? UpdateExerciseSession(ExerciseSession session)
    {
        return exerciseRepository.UpdateExerciseSession(session);
    }
}
