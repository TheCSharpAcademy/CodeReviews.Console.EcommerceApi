using ExerciseTracker.hasona23.Models;
using Spectre.Console;

namespace ExerciseTracker.hasona23.Handlers;

public static class VisualisationHandler
{
    public static void DisplayExercisesTable(List<Exercise> exercises)
    {
        Console.Clear();
        var table = new Table().Title("Exercises");
        table.Border(TableBorder.Rounded);
        table.BorderStyle(new Style(Color.White));
        table.AddColumns("[yellow]Id[/]", "[yellow]Day[/]", "[yellow]Start[/]", "[yellow]End[/]",
            "[yellow]Duration[/]","[yellow]Description[/]");
        foreach (var exercise in exercises)
        {
            table.AddRow($"[darkturquoise]{exercises.IndexOf(exercise)}[/]",
                $"[darkturquoise]{exercise.Start:dd/MM/yyyy}[/]",
                $"[darkturquoise]{exercise.Start:hh:mm}[/]",
                $"[darkturquoise]{exercise.End:hh:mm}[/]",
                $"[darkturquoise]{exercise.Duration.Hours}h : {exercise.Duration.Minutes}m[/]",
                $"[darkturquoise]{exercise.Description}[/]");
        }

        table.Expand();
        foreach (var tableColumn in table.Columns)
        {
            tableColumn.Alignment = Justify.Center;
        } 
        
        AnsiConsole.Write(table);
        MenuBuilder.Pause();
    }
}