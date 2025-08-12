namespace ExerciseTracker.samggannon.Services;

public interface IExerciseService
{
    public void InsertSession();
    public void EditSession();
    public void GetAllSessions();
    public void DeleteSessionById();
    public void SetRepository(bool isResistanceTraining);
}
