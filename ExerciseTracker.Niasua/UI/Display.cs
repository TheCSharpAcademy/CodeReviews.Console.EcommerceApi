using ExerciseTracker.Niasua.Models;
using Spectre.Console;

namespace ExerciseTracker.Niasua.UI;

public static class Display
{
    public static void ShowExercises(List<Exercise> exercises)
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("[red]Id[/]");
        table.AddColumn("[yellow]Date Start[/]");
        table.AddColumn("[blue]Date End[/]");
        table.AddColumn("[green]Duration[/]");
        table.AddColumn("[cyan]Comments[/]");

        foreach (var exercise in exercises)
        {
            table.AddRow(
                exercise.Id.ToString(),
                exercise.DateStart.ToString("g"),
                exercise.DateEnd.ToString("g"),
                exercise.Duration.ToString("g"),
                string.IsNullOrWhiteSpace(exercise.Comments) ? "[grey]None[/]" : exercise.Comments
                );
        }

        AnsiConsole.Write(table);
    }

    public static void ShowExercise(Exercise exercise)
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("[red]Id[/]");
        table.AddColumn("[yellow]Date Start[/]");
        table.AddColumn("[blue]Date End[/]");
        table.AddColumn("[green]Duration[/]");
        table.AddColumn("[cyan]Comments[/]");

        
        table.AddRow(
            exercise.Id.ToString(),
            exercise.DateStart.ToString("g"),
            exercise.DateEnd.ToString("g"),
            exercise.Duration.ToString("g"),
            string.IsNullOrWhiteSpace(exercise.Comments) ? "[grey]None[/]" : exercise.Comments
            );
        
        AnsiConsole.Write(table);
    }
}
