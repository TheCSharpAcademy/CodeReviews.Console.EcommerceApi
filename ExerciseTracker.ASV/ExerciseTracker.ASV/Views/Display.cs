using Spectre.Console;
using ExerciseTracker.ASV.Models;

namespace ExerciseTracker.ASV.Views;

public class Display : IDisplay
{
    public void DisplayWorkouts(List<ExerciseData> exercises, string[] columns, string title)
    {
        var table = new Table();
        table.Title = new TableTitle(title);
        foreach (var column in columns)
        {
            table.AddColumn(column);
        }
        foreach (var exercise in exercises)
        {
            table.AddRow(exercise.Id.ToString(), exercise.DateStart.ToString(), exercise.DateEnd.ToString(), exercise.Duration.ToString(), exercise.Description);
        }
        AnsiConsole.Write(table);
    }

    public string GetSelection(string title, string[] choices)
    {
        var selectedCategory = AnsiConsole.Prompt(new SelectionPrompt<string>().Title(title).AddChoices(choices).HighlightStyle(new Style(foreground: Color.Blue)));
        return selectedCategory;
    }
}