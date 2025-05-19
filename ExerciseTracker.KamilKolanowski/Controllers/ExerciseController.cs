using ExerciseTracker.KamilKolanowski.Models;
using ExerciseTracker.KamilKolanowski.Repositories;
using ExerciseTracker.KamilKolanowski.Services;

namespace ExerciseTracker.KamilKolanowski.Controllers;

public class ExerciseController
{
    private readonly ExerciseService _service;

    public ExerciseController(ExerciseService service)
    {
        _service = service;
    }

    public void AddExercise()
    {
        _service.AddExercise();
    }

    public void DeleteExercise()
    {
        _service.DeleteExercise();
    }

    public void UpdateExercise()
    {
        _service.UpdateExercise();
    }

    public void ReadExercises()
    {
        _service.ReadExercises();
    }
}
