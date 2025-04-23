using Spectre.Console;

namespace ExerciseTracker.UserInterface;

public abstract class BaseMenu
{
    public abstract Task ShowMenuAsync();

    public static void PressAnyKeyToContinue()
    {
        AnsiConsole.MarkupLine("[green]Press Any Key To Continue[/]");
        Console.ReadKey();
        Console.Clear();
    }
}