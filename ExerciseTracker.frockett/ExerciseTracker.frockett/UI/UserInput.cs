using ExerciseTracker.frockett.Models;
using Spectre.Console;
using System.Globalization;

namespace ExerciseTracker.frockett.UI;

public class UserInput
{
    public ExerciseSession GetNewSession()
    {
        DateTime startTime = GetDateTime("Enter session start in format yyyy-MM-dd HH:mm: ");
        DateTime endTime = GetDateTime("Enter session end: ");

        while (startTime > endTime)
        {
            AnsiConsole.MarkupLine("[red]Invalid input, the session can't end before it starts![/]");
            startTime = GetDateTime("Enter session start in format yyyy-MM-dd HH:mm: ");
            endTime = GetDateTime("Enter session end in format yyyy-MM-dd HH:mm: ");
        }

        string? comment = GetComment();

        return new ExerciseSession { StartTime = startTime, EndTime = endTime, Comments = comment };
    }

    private DateTime GetDateTime(string prompt)
    {
        DateTime dateTime;
        string validFormat = "yyyy-MM-dd HH:mm";

        var sDateTime = AnsiConsole.Ask<string>(prompt);

        while (!DateTime.TryParseExact(sDateTime, validFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
        {
            AnsiConsole.WriteLine("\nIncorrect date/time format.");
            sDateTime = AnsiConsole.Ask<string>(prompt);
        }

        return dateTime;
    }

    private TimeSpan GetTimeOnly(string prompt)
    {
        TimeSpan timeSpan;
        string validFormat = "HH\\:mm";

        var input = AnsiConsole.Ask<string>(prompt);

        while (!TimeSpan.TryParseExact(input, validFormat, CultureInfo.InvariantCulture, out timeSpan))
        {
            AnsiConsole.WriteLine("\nIncorrect time format.");
            input = AnsiConsole.Ask<string>(prompt);
        }

        return timeSpan;
    }

    private string? GetComment() 
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("(Optional) Comments: ").AllowEmpty());
    }
}
