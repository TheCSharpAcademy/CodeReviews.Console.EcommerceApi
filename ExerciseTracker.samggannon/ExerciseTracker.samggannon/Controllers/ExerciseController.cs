using ExerciseTracker.samggannon.Services;

namespace ExerciseTracker.samggannon.Controllers;

public class ExerciseController
{
    private readonly IExerciseService? _exerciseService;

    public ExerciseController()
    {
        _exerciseService = new ExerciseService();
    }

    public ExerciseController(IExerciseService exerciseService, bool isResistanceTraining)
    {
        _exerciseService = exerciseService;
        _exerciseService.SetRepository(isResistanceTraining);
    }

    public void InsertSession()
    {
        _exerciseService.InsertSession();
    }

    public void GetAllSessions()
    {
        _exerciseService.GetAllSessions();
    }

    public void EditSession()
    {
        _exerciseService.EditSession();
    }

    public void DeleteSessionById()
    {
        _exerciseService.DeleteSessionById();
    }

    internal void SetRepository(bool isResistanceTraining)
    {
        _exerciseService.SetRepository(isResistanceTraining);
    }
}
