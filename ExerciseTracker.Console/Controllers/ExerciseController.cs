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
        var exercises = await _service.GetAllExercisesAsync();
        Display.ShowExercises(exercises);

        var id = UserInputHandler.GetId();
        if (id == null) return;

        if (!await ExerciseValidator.ExerciseExistsById(id.Value))
        {
            AnsiConsole.MarkupLine("[red]Failed to delete exercise.[/]");
            return;
        }

        await _service.DeleteExerciseAsync(id.Value);
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

    public async Task UpdateExerciseAsyc()
    {
        var exercises = await _service.GetAllExercisesAsync();
        Display.ShowExercises(exercises);

        var id = UserInputHandler.GetId();
        if (id == null) return;

        var exercise = await _service.GetExerciseByIdAsync(id.Value);

        if (exercise == null)
        {
            AnsiConsole.MarkupLine("[red]No exercise found.[/]");
            return;
        }

        AnsiConsole.MarkupLine($"Current Start Date: [blue]{exercise.DateStart:g}[/]");
        var newStart = UserInputHandler.GetDateTime("New Start Date (leave empty to keep current):");
        if (newStart != null) exercise.DateStart = newStart.Value;

        AnsiConsole.MarkupLine($"Current End Date: [blue]{exercise.DateEnd:g}[/]");
        var newEnd = UserInputHandler.GetDateTime("New End Date (leave empty to keep current):");
        if (newEnd != null) exercise.DateEnd = newEnd.Value;

        var newComments = AnsiConsole.Ask<string>("New comments (leave empty to keep current):");
        if (!string.IsNullOrWhiteSpace(newComments))
            exercise.Comments = newComments;

        var success = await _service.UpdateExerciseAsync(exercise);
        if (success)
            AnsiConsole.MarkupLine("[green]Exercise successfully updated.[/]");
        else
            AnsiConsole.MarkupLine("[red]Update failed. Please check the input data.[/]");

        await _service.UpdateExerciseAsync(exercise);
        AnsiConsole.MarkupLine("[green]Exercise successfully updated.[/]");
    }
}
