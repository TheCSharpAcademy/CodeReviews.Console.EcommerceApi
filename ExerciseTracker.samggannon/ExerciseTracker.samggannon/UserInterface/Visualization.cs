using ExerciseTracker.samggannon.Data.Models;
using Spectre.Console;

namespace ExerciseTracker.samggannon.UserInterface;

internal class Visualization
{
    internal static void ShowTable(IEnumerable<Exercise> exercises)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Exercise Type");
        table.AddColumn("start time");
        table.AddColumn("end time");
        table.AddColumn("duration");
        table.AddColumn("Comments");

        foreach (var exercise in exercises)
        {
            table.AddRow(
                exercise.Id.ToString(),
                exercise.Type,
                exercise.DateStart.ToString(),
                exercise.DateEnd.ToString(),
                exercise.Duration.ToString(),
                exercise.Comments
                );
        }

        AnsiConsole.Write(table);
    }
}
