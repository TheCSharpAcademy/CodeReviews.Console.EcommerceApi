using ExerciseTracker.Models;
using Spectre.Console;
using System.Globalization;

namespace ExerciseTracker;

internal class UserInput
{
    public static DateTime GetDateInputs()
    {
        DateTime input;
        while (!DateTime.TryParseExact(AnsiConsole.Ask<string>("Enter a date (dd-mm-yy hh:mm)"), "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out input))
        {
            AnsiConsole.MarkupLine("Invalid date format (dd-mm-yy hh:mm)");
        }
        return input;
    }

    internal static string GetCommentInput()
    {
        var comment = AnsiConsole.Ask<string>("Enter a comment:").Trim();
        return comment;
    }

    internal static int GetExerciseId(string message)
    {
        int input;
        while (!int.TryParse(AnsiConsole.Ask<string>($"Enter the id of the exercise you want to {message}"), out input))
        {
            AnsiConsole.MarkupLine("Enter a valid number");
        }
        return input;
    }
}
