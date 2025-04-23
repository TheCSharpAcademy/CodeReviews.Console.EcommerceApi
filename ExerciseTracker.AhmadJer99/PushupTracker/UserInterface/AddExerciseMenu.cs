using ExerciseTracker.Controllers;
using ExerciseTracker.Models;
using Spectre.Console;
using ExerciseTracker.Services;
using ConsoleTableExt;


namespace ExerciseTracker.UserInterface;

public class AddExerciseMenu : BaseMenu
{
    private readonly ExerciseController _exerciseController;
    public AddExerciseMenu(ExerciseController exerciseController)
    {
        _exerciseController = exerciseController;
    }

    public override async Task ShowMenuAsync()
    {
        DateTime date;
        int reps;
        Console.Clear();
        AnsiConsole.MarkupLine("[teal]Add Exercise Menu[/]\n");
        Console.WriteLine("Please enter the details of the exercise you want to add:");

        try
        {
            Console.Write("Enter date (MM-dd-yyyy HH:mm)  (e.g., 04-10-2024 04:30) Or (Q to cancel): ");
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
        string? comments = AnsiConsole.Ask<string>("Enter a comment Or (Q to cancel): ");

        var pushup = new Pushup
        {
            Date = date,
            Reps = reps,
            Comments = comments
        };

        await _exerciseController.CreateExerciseAsync(pushup);

        TableVisualisationEngine<Pushup>.ViewAsTable(
            [pushup],
            tableAligntment: TableAligntment.Center,
            ["Id","Date", "Reps", "Comments"],
            "Exercise added successfully!"
        );
        PressAnyKeyToContinue();
    }
}