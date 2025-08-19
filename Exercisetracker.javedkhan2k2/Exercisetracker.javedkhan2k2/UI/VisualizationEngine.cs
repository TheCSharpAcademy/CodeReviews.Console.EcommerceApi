using System.Diagnostics.CodeAnalysis;
using Exercisetacker.Entities;
using Spectre.Console;
namespace Exercisetacker.UI;

internal class VisualizationEngine
{

    internal static void DisplayContinueMessage()
    {
        AnsiConsole.Markup($"\n[yellow]Press [blue]Enter[/] To Continue[/]\n");
        Console.ReadLine();
    }

    public static Table CreateTable(string title, string footer)
    {
        var table = new Table();
        table.Title($"[yellow]{title}[/]");
        table.Caption(footer);
        table.Border = TableBorder.Square;
        table.ShowRowSeparators = true;
        return table;
    }

    internal static void DisplayCancelOperation()
    {
        AnsiConsole.Markup("[maroon]The Operation is Canceled by the User.[/]\n");
        DisplayContinueMessage();
    }

    internal static void DisplayFailureMessage(string message)
    {
        AnsiConsole.Markup($"[maroon]{message}[/]");
        DisplayContinueMessage();
    }

    internal static void DisplaySuccessMessage(string message)
    {
        AnsiConsole.Markup($"[green]{message}[/]");
        DisplayContinueMessage();
    }

    internal static void DisplayDateTimeError()
    {
        AnsiConsole.Markup("[maroon]The End Date Time should be later than Start Date Time.[/]\n");
        DisplayContinueMessage();
    }

    internal static void DisplayAllJoggings(List<Jogging>? joggings, [AllowNull] string title)
    {
        if (title == null)
        {
            title = "";
        }
        var table = CreateTable(title, $"[yellow]Displaying [blue]{joggings.Count()}[/] records[/]");
        table.AddColumns(["[green]Id[/]", "[green]Start Time[/]", "[green]End Time[/]", "[green]Duration[/]", "[green]Comments[/]"]);
        foreach (var jogging in joggings)
        {
            table.AddRow(jogging.Id.ToString(), jogging.DateStart.ToString(), jogging.DateEnd.ToString(), jogging.Duration.ToString(), jogging.Comments);
        }
        AnsiConsole.Write(table);
    }

}