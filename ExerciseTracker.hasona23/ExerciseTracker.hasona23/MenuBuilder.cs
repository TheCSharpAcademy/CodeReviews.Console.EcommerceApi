using ExerciseTracker.hasona23.Enums;
using Spectre.Console;

namespace ExerciseTracker.hasona23;

public static class MenuBuilder
{
    public static void Pause()
    {
        AnsiConsole.MarkupLine("[yellow]Press any key to continue...[/]");
        Console.ReadLine();
    }

    public static ExerciseOptions GetExerciseOption()
    {
        return AnsiConsole.Prompt(new SelectionPrompt<ExerciseOptions>()
            .Title("[yellow]Choose Options[/]")
            .AddChoices(Enum.GetValues<ExerciseOptions>())
            .HighlightStyle(new Style(Color.Yellow)));
    }

    public static Options GetOptions()
    {
        return AnsiConsole.Prompt(new SelectionPrompt<Options>()
            .Title("[yellow]Choose Option[/]")
            .AddChoices(Enum.GetValues<Options>())
            .HighlightStyle(new Style(Color.Yellow)));
    }

    public static void ShowHelpMenu()
    {
        AnsiConsole.MarkupLine("[yellow]--Help--[/]");
        AnsiConsole.MarkupLine($"[darkturquoise]{Options.Exercises.ToString()}[/] -> Manage Exercises (Add,Delete,Update,Read)");
        AnsiConsole.MarkupLine($"[darkturquoise]{Options.Exit.ToString()}[/] -> Exit App");
        Pause();
    }
}