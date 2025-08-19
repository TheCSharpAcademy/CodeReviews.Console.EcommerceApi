using Spectre.Console;
using ExerciseTracker.DTO;

namespace ExerciseTracker.UI;

public class UserInput
{
    public string Menu(string title, string color, List<string> mainOptions)
    {
        Console.Clear();

        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"[{color}]{title}[/]")
            .PageSize(10)
            .AddChoices(mainOptions));

        return selection;
    }

    public string GetString(string message)
    {
        return AnsiConsole.Prompt(new TextPrompt<string>($@"[bold blue]{message} [/]"));
    }

    public void ShowMessage(string message, string color)
    {
        AnsiConsole.MarkupLine($"[{color}]{message}[/]\n");
    }

    public void PressKey(string message)
    {
        ShowMessage(message, "blue");
        Console.ReadKey();
        Console.Clear();
    }

    public void MessageAndPressKey(string message, string color)
    {
        ShowMessage(message, color);
        PressKey("Press any key to continue");
    }

    public void ShowTable(string title, string[] columns, List<RecordDto> records)
    {
        var table = new Table();
        table.Title(title);
        foreach (var column in columns)
        {
            table.AddColumn(column);
        }

        foreach (var record in records)
        {
            table.AddRow(record.Column1, record.Column2);
        }

        AnsiConsole.Write(table);
    }

    public bool Choice(string prompt)
    {
        var confirmation = AnsiConsole.Prompt(
    new TextPrompt<bool>(prompt)
        .AddChoice(true)
        .AddChoice(false)
        .DefaultValue(true)
        .WithConverter(choice => choice ? "y" : "n"));
        return confirmation;
    }
}