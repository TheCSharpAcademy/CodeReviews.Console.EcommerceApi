using Spectre.Console;

namespace ExerciseTracker.iamnikitakostin;
internal class ConsoleHelper
{
    protected static void DisplayMessage(string message, string color = "yellow")
    {
        AnsiConsole.MarkupLine($"[{color}]{message}[/]");
        AnsiConsole.WriteLine("Press any key to continue");
        AnsiConsole.Console.Input.ReadKey(false);
    }

    protected static bool ConfirmDeletion(string itemName)
    {

        AnsiConsole.Clear();
        var confirm = AnsiConsole.Confirm($"Are you sure you want to delete [red]{itemName}[/]?");
        return confirm;
    }

    protected static void SuccessMessage(string message)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[green]{message}[/]");
        AnsiConsole.WriteLine("Press any key to continue");
        AnsiConsole.Console.Input.ReadKey(false);
    }

    protected static void ErrorMessage(string message)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[red]{message}[/]");
        AnsiConsole.WriteLine("Press any key to continue");
        AnsiConsole.Console.Input.ReadKey(false);
    }
}