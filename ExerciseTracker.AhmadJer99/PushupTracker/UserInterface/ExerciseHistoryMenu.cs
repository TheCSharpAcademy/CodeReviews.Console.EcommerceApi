using ExerciseTracker.Services;
using ExerciseTracker.Controllers;
using ExerciseTracker.Models;
using ConsoleTableExt;
using Spectre.Console;

namespace ExerciseTracker.UserInterface;

public class ExerciseHistoryMenu : BaseMenu
{
    private readonly ExerciseController _exerciseController;

    public ExerciseHistoryMenu(ExerciseController exerciseController)
    {
        _exerciseController = exerciseController;
    }
    public override async Task ShowMenuAsync()
    {
        try
        {
            var result = _exerciseController.GetAllExercisesAsync().Result;
            var _pushups = result.Data ?? [];
            if (result.Success)
            {
                Console.WriteLine(result.Message);
                await ShowExerciseHistoryAsync(_pushups);
            }
            else
            {
                Console.WriteLine(result.Message);
            }
        }
        catch (AggregateException ex)
        {
            Console.WriteLine("An error occurred while fetching exercises: " + ex.InnerException?.Message);
            return;
        }
    }

    private async Task ShowExerciseHistoryAsync(List<Pushup> pushups)
    {
        Console.Clear();
        if (pushups == null || pushups.Count == 0)
        {
            Console.WriteLine("No exercise history found.");
            return;
        }
        TableVisualisationEngine<Pushup>.ViewAsTable(pushups, TableAligntment.Left, ["ID", "Date", "Reps", "Comments"], "Exercise History");
        await PromptActionOnRowAsync();
    }

    private async Task PromptActionOnRowAsync()
    {
        while (true)
        {
            Console.Write("\nEnter action (e.g., E1 to edit row with id=1, D1 to delete row with id=1, Q to quit): ");
            var input = Console.ReadLine()?.Trim().ToUpper();

            if (string.IsNullOrEmpty(input))
                continue;

            if (input == "Q")
                return;

            if (input.StartsWith('E') && int.TryParse(input[1..], out int editId))
            {
                if (await CheckIdExistsAsync(editId))
                    await HandleRowEditAsync(editId);
                break;
            }
            else if (input.StartsWith('D') && int.TryParse(input[1..], out int deleteId))
            {
                if (await CheckIdExistsAsync(deleteId))
                    await HandleRowDeleteAsync(deleteId);
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Try again.");
            }
        }
        await ShowMenuAsync();
    }

    private async Task<bool> CheckIdExistsAsync(int editId)
    {
        try
        {
            var result = await _exerciseController.GetExerciseByIdAsync(editId);
            if (!result.Success)
            {
                Console.WriteLine(result.Message);
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }

    private async Task HandleRowEditAsync(int editId)
    {
        // Initialize data to avoid CS0165 error
        DateTime date;
        int reps;
        try
        {
            Console.Write("Enter new date (MM-dd-yyyy HH:mm)  (e.g., 04-10-2024 04:30) Or (Q to cancel): ");
            date = Validation.ValidateDateInput(Console.ReadLine());
        }
        catch (ArgumentException ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            PressAnyKeyToContinue();
            return;
        }
        try
        {
            Console.Write("Enter number of reps: Or (Q to cancel): ");
            reps = Validation.ValidateIntInput(Console.ReadLine());
        }
        catch (ArgumentException ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            PressAnyKeyToContinue();
            return;
        }

        string? comments = AnsiConsole.Ask<string>("Enter the new comment Or (Q to cancel): ");
        if (comments?.ToLower().Trim() == "q")
        {
            AnsiConsole.MarkupLine("[red]Operation cancelled.[/]");
            PressAnyKeyToContinue();
            return;
        }

        var updatedPushup = new Pushup
        {
            Id = editId,
            Date = date,
            Reps = reps,
            Comments = comments
        };
        try
        {
            var result = await _exerciseController.UpdateExerciseAsync(editId, updatedPushup);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[green]{result.Message}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]{result.Message}[/]");
            }

            PressAnyKeyToContinue();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            PressAnyKeyToContinue();
        }
    }

    private async Task HandleRowDeleteAsync(int deleteId)
    {
        try
        {
            var result = await _exerciseController.DeleteExerciseAsync(deleteId);
            if (result.Success)
            {
                AnsiConsole.MarkupLine($"[green]{result.Message}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]{result.Message}[/]");
            }
            PressAnyKeyToContinue();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            PressAnyKeyToContinue();
        }
    }
}