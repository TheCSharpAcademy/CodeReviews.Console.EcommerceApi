using ExerciseTracker.samggannon.Data.Models;

namespace ExerciseTracker.samggannon.Data.Repositories;

public interface IExerciseRepository
{
    public void Add(Exercise entity);
    public void Delete(Exercise entity);
    public List<Exercise> GetAllSessions();
    public Exercise GetSessionById(int sessionId);
    public void Update(Exercise entity);
}
