using ExerciseTracker.Models;
using Spectre.Console;

namespace ExerciseTracker;

class DataOutput
{
    internal static void PrintAllExercices(IExercices[] exercices)
    {
        Table table = new();
        if (exercices.Length != 0)
        {
            table.Title($"All {exercices[0].GetType().Name}");
            table.AddColumns("Start", "End", "Duration", "Comment");
            table.ShowRowSeparators().SimpleBorder();
            foreach (IExercices exercice in exercices)
            {
                table.AddRow(exercice.Start.ToString("yyyy.MM.dd HH:mm:ss"), exercice.End.ToString("yyyy.MM.dd HH:mm:ss"), exercice.Duration.Value.ToString(@"hh\:mm\:ss"), exercice.Comment);
                table.AddEmptyRow();
            }
            AnsiConsole.Write(table);
        }
        else AnsiConsole.WriteLine("No data found in the database.");
    }

    internal static void PrintSingleExercice(IExercices exercice)
    {
        Panel panel = new($"Start: {exercice.Start}\nEnd: {exercice.End}\nComment: {exercice.Comment}");
        panel.Header($"Field Tour n°{exercice.Id}");
        panel.Padding(5, 5, 5, 5);
        AnsiConsole.Write(panel);
    }
}