using ExerciseTracker.Models;
using ExerciseTracker.Services;
using Spectre.Console;

namespace ExerciseTracker.Controllers;

internal class ExerciseController
{
    private readonly ExerciseService _service;

    public ExerciseController(ExerciseService service)
    {
        _service = service;
    }

    public void AddExercise()
    {
        AnsiConsole.MarkupLine("Creating exercise:");
        _service.AddExercise();
    }

    public void Update()
    {
        AnsiConsole.MarkupLine("Updating exercise:");
        _service.UpdateExercise();
    }

    public void Delete()
    {
        _service.DeleteExercise();
    }

    public Exercise Get()
    {
        return _service.GetExercise();
    }

    public List<Exercise> GetAll()
    {
        return _service.GetAllExercises();
    }
}
