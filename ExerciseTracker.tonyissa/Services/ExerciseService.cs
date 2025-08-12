using ExerciseTracker.tonyissa.Models;
using ExerciseTracker.tonyissa.Repositories;

namespace ExerciseTracker.tonyissa.Services;

public class ExerciseService
{
    private readonly IExerciseRepository<ExerciseSession> _repository;

    public ExerciseService(IExerciseRepository<ExerciseSession> repository)
    {
        _repository = repository;
    }

    public List<ExerciseSession> GetExerciseLog()
    {
        var log = _repository.GetAllSessions();
        return log.ToList();
    }

    public void AddExerciseSession(ExerciseSession session)
    {
        _repository.AddSession(session);
    }

    public void UpdateSession(ExerciseSession session)
    {
        _repository.ModifySession(session);
    }

    public void RemoveSession(ExerciseSession session)
    {
        _repository.DeleteSession(session);
    }
}