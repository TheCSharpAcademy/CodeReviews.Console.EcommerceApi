using Kmakai.ExerciseTracker.Models;
using Spectre.Console;

namespace Kmakai.ExerciseTracker;

public class Display
{
    public static void DisplayTable(List<Exercise> list)
    {
        var table = new Table();

        table.AddColumn("Id");
        table.AddColumn("Start");
        table.AddColumn("End");
        table.AddColumn("Duration");
        table.AddColumn("Comments");

        foreach (var item in list)
        {
            table.AddRow(item.Id.ToString(), item.DateStart.ToString(), item.DateEnd.ToString(), item.Duration.ToString("hh':'mm"), item.Comments!);
        }

        AnsiConsole.Write(table);
    }

    public static void DisplayExercise(Exercise exercise)
    {
        var table = new Table();

        table.AddColumn("Id");
        table.AddColumn("Start");
        table.AddColumn("End");
        table.AddColumn("Duration");
        table.AddColumn("Comments");

        table.AddRow(exercise.Id.ToString(), exercise.DateStart.ToString(), exercise.DateEnd.ToString(), exercise.Duration.ToString("hh':'mm"), exercise.Comments!);

        AnsiConsole.Write(table);

    }
}
