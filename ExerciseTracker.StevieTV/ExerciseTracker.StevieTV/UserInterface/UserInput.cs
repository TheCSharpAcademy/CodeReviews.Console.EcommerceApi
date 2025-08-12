using System.Globalization;
using Spectre.Console;

namespace ExerciseTracker.StevieTV.UserInterface;

public class UserInput
{
    public static string GetDate(DateTime? currentDate = null)
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("Exercise Date (dd/mm/yy):")
            .DefaultValue(currentDate != null ? currentDate.Value.ToShortDateString() : DateTime.Now.ToString("dd/MM/yy"))
            .Validate(
                x => DateTime.TryParseExact(x, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _),
                "Please enter a valid Exercise date in the format dd/mm/yy"));
    }

    public static string GetTime(bool isStartTime, DateTime? startTime = null)
    {
        if (isStartTime)
        {
            return AnsiConsole.Prompt(new TextPrompt<string>("Start Exercise Time (hh:mm):")
                .DefaultValue(startTime != null ? startTime.Value.ToShortTimeString() : DateTime.Now.ToString("HH:mm"))
                .Validate(
                    x => DateTime.TryParseExact(x, @"H\:m", CultureInfo.InvariantCulture, DateTimeStyles.None, out _),
                    "Please enter a valid Exercise time in the format hh:mm")
                );
        }
        return AnsiConsole.Prompt(new TextPrompt<string>("End Exercise Time (hh:mm):")
            .DefaultValue(startTime != null ? startTime.Value.ToShortTimeString() : DateTime.Now.ToString("HH:mm"))
            .Validate(
                x => DateTime.TryParseExact(x, @"H\:m", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedTime) && TimeOnly.FromDateTime(parsedTime) > TimeOnly.FromDateTime(startTime.Value),
                "Please enter a valid Exercise time in the format hh:mm, that is also later that your exercise start time")
        );
    }

    public static string GetComment(string currentComment = "")
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("Exercise comment (optional):")
            .DefaultValue(currentComment));
    }
}