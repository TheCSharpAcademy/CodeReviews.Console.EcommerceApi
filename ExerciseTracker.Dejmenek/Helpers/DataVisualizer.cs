using ExerciseTracker.Dejmenek.Models;
using Spectre.Console;

namespace ExerciseTracker.Dejmenek.Helpers;
public static class DataVisualizer
{
    public static void DisplayExercises(List<ExerciseReadDTO> exerciseDtos)
    {
        if (exerciseDtos.Count == 0)
        {
            AnsiConsole.MarkupLine("No exercises found.");
            return;
        }

        var table = new Table().Title("EXERCISES");

        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Duration in minutes");
        table.AddColumn("Comments");

        foreach (var exercise in exerciseDtos)
        {
            table.AddRow(exercise.StartTime.ToString("yyyy-MM-dd HH:mm"), exercise.EndTime.ToString("yyyy-MM-dd HH:mm"), exercise.Duration, exercise.Comments ?? "");
        }

        AnsiConsole.Write(table);
    }
}
