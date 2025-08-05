using ExerciseTracker.Niasua.Services;
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
        var idInput = AnsiConsole.Ask<string>("Type Exercise's ID:");
        if (!int.TryParse(idInput, out int id))
        {
            AnsiConsole.MarkupLine("[red]Invalid ID.[/]");
            return;
        }

        if (!await ExerciseValidator.ExerciseExistsById(id))
        {
            AnsiConsole.MarkupLine("[red]Failed to delete exercise.[/]");
            return;
        }

        await _service.DeleteExerciseAsync(id);
        AnsiConsole.MarkupLine("[green]Exercise successfully deleted.[/]");
    }
}
