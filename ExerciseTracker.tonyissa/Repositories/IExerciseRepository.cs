namespace ExerciseTracker.tonyissa.Repositories;

public interface IExerciseRepository<T>
{
    public IEnumerable<T> GetAllSessions();
    public void AddSession(T session);
    public void DeleteSession(T session);
    public void ModifySession(T session);
}