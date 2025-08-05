using ExerciseTracker.Niasua.Services;
using ExerciseTracker.Niasua.UI;
using ExerciseTracker.Niasua.UserInput;
using ExerciseTracker.Niasua.Validators;
using Spectre.Console;

namespace ExerciseTracker.Niasua.Controllers;

public class ExerciseController
{
    private readonly ExerciseService _service;

    public ExerciseController(ExerciseService service)
    {
        _service = service;
    }
    
    public async Task AddExerciseAsync()
    {
        var exercise = UserInputHandler.GetExerciseInput();

        var success = await _service.CreateExerciseAsync(exercise);

        if (success)
            AnsiConsole.MarkupLine("[green]Exercise successfully added.[/]");
        else
            AnsiConsole.MarkupLine("[red]Failed to add exercise.[/]");
    }

    public async Task DeleteExerciseAsync()
    {
        var id = UserInputHandler.GetId();
        if (id == null) return;

        if (!await ExerciseValidator.ExerciseExistsById(id.Value))
        {
            AnsiConsole.MarkupLine("[red]Failed to delete exercise.[/]");
            return;
        }

        await _service.DeleteExerciseAsync(id);
        AnsiConsole.MarkupLine("[green]Exercise successfully deleted.[/]");
    }

    public async Task GetAllExercisesAsync()
    {
        var exercises = await _service.GetAllExercisesAsync();

        if(exercises == null || exercises.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No exercises found.[/]");
            return;
        }

        Display.ShowExercises(exercises);
    }

    public async Task GetExerciseByIdAsync()
    {
        var id = UserInputHandler.GetId();
        if (id == null) return;

        var exercise = await _service.GetExerciseByIdAsync(id.Value);

        if (exercise == null)
        {
            AnsiConsole.MarkupLine("[red]No exercise found.[/]");
            return;
        }

        Display.ShowExercise(exercise);
    }
}
