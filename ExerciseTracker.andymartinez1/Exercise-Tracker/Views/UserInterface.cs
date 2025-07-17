using Exercise_Tracker.Models;
using Exercise_Tracker.Services;
using Spectre.Console;

namespace Exercise_Tracker.Views;

public static class UserInterface
{
    private static readonly IExerciseService _service;

    public static void ShowAllExercises(List<Exercise> exercises)
    {
        var table = new Table()
            .Title("All Exercises")
            .AddColumn("ID")
            .AddColumn("Start Time")
            .AddColumn("End Time")
            .AddColumn("Duration")
            .AddColumn("Comments");
        foreach (var exercise in exercises)
        {
            table.AddRow(
                exercise.Id.ToString(),
                exercise.StartTime.ToString("g"),
                exercise.EndTime.ToString("g"),
                $"{Math.Floor(exercise.Duration.TotalHours)} hours {exercise.Duration.TotalMinutes % 60} minutes",
                exercise.Comments ?? "N/A"
            );
        }

        table.Expand();
        AnsiConsole.Write(table);
    }

    public static void ShowExerciseDetails(Exercise exercise)
    {
        var panel = new Panel(
            $"Start Time: {exercise.StartTime:g} \nEnd Time: {exercise.EndTime:g} \nDuration: {Math.Floor(exercise.Duration.TotalHours)} hours {exercise.Duration.TotalMinutes % 60} minutes \nComments: {exercise.Comments}"
        )
            .Header($"Exercise Details for ID: {exercise.Id}")
            .BorderStyle(Style.Parse("green"));

        panel.Padding = new Padding(2);
        panel.Expand();

        AnsiConsole.Write(panel);
    }
}
