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
        try
        {
            var exercise = UserInputHandler.GetExerciseInput();
            if (exercise == null)
                return;

            var success = await _service.CreateExerciseAsync(exercise);

            if (success)
                AnsiConsole.MarkupLine("\n[green]Exercise successfully added.[/]");
            else
                AnsiConsole.MarkupLine("\n[red]Failed to add exercise.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]Error:[/] {ex.Message}");
            Console.ReadKey();
        }
    }

    public async Task DeleteExerciseAsync()
    {
        try
        {
            var exercises = await _service.GetAllExercisesAsync();
            Display.ShowExercises(exercises);

            var id = UserInputHandler.GetId();
            if (id == null) return;

            if (!await ExerciseValidator.ExerciseExistsById(id.Value, _service))
            {
                AnsiConsole.MarkupLine("\n[red]Failed to delete exercise.[/]");
                return;
            }

            var confirmation = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"\nAre you sure you want to delete [yellow]Exercise {id}?[/]")
                    .AddChoices(new[] {
                        "Yes", "No"
                    }));

            if (confirmation == "Yes")
            {
                await _service.DeleteExerciseAsync(id.Value);
                AnsiConsole.MarkupLine("\n[green]Exercise successfully deleted.[/]");
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]Error:[/] {ex.Message}");
            Console.ReadKey();
        }
    }

    public async Task GetAllExercisesAsync()
    {
        try
        {
            var exercises = await _service.GetAllExercisesAsync();

            if (exercises == null || exercises.Count == 0)
            {
                AnsiConsole.MarkupLine("\n[red]No exercises found.[/]");
                return;
            }

            Display.ShowExercises(exercises);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]Error:[/] {ex.Message}");
            Console.ReadKey();
        }
    }
    public async Task GetExerciseByIdAsync()
    {
        try
        {
            var id = UserInputHandler.GetId();
            if (id == null) return;

            var exercise = await _service.GetExerciseByIdAsync(id.Value);

            if (exercise == null)
            {
                AnsiConsole.MarkupLine("\n[red]No exercise found.[/]");
                return;
            }

            Display.ShowExercise(exercise);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]Error:[/] {ex.Message}");
            Console.ReadKey();
        }
    }

    public async Task UpdateExerciseAsync()
    {
        try
        {
            var exercises = await _service.GetAllExercisesAsync();
            Display.ShowExercises(exercises);

            var id = UserInputHandler.GetId();
            if (id == null) return;

            var exercise = await _service.GetExerciseByIdAsync(id.Value);

            if (exercise == null)
            {
                AnsiConsole.MarkupLine("\n[red]No exercise found.[/]");
                return;
            }

            AnsiConsole.MarkupLine($"Current Start Date: [blue]{exercise.DateStart:g}[/]");
            var newStart = UserInputHandler.GetDateTime("New Start Date (leave empty to keep current):");
            if (newStart != null) exercise.DateStart = newStart.Value;

            AnsiConsole.MarkupLine($"Current End Date: [blue]{exercise.DateEnd:g}[/]");
            var newEnd = UserInputHandler.GetDateTime("New End Date (leave empty to keep current):");
            if (newEnd != null) exercise.DateEnd = newEnd.Value;

            AnsiConsole.MarkupLine("New Comments: (leave empty to keep current):");
            var newComments = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newComments))
                exercise.Comments = newComments;

            var success = await _service.UpdateExerciseAsync(exercise);
            if (success)
                AnsiConsole.MarkupLine("\n[green]Exercise successfully updated.[/]");
            else
                AnsiConsole.MarkupLine("\n[red]Update failed. Please check the input data.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]Error:[/] {ex.Message}");
            Console.ReadKey();
        }
    }
}
