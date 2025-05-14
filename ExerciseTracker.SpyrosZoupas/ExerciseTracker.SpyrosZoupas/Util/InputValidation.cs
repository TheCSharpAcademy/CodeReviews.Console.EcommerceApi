using Spectre.Console;
using System.Globalization;

namespace UserInterface.SpyrosZoupas.Util;

public class InputValidation
{
    public static DateTime GetDateTimeValue(string message)
    {
        string dateTime = AnsiConsole.Prompt(
            new TextPrompt<string>(message)
            .Validate(input =>
            {
                return DateTime.TryParseExact(input, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                    ? ValidationResult.Success()
                    : DateTime.TryParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                    ? ValidationResult.Success()
                    : ValidationResult.Error($"[white on red]Invalid format. Please enter any DateTime values in dd-MM-yyyy HH:mm:ss foramt. Example: 20-01-2025 13:00:00.[/]");
            }));

        if (!DateTime.TryParseExact(dateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result)) 
        {
            result = DateTime.ParseExact(dateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            AnsiConsole.MarkupLine("[yellow3_1]Time of DateTime value not set; Defaulting to 00:00:00.[/]");
            return result.Date;
        }

        return result;
    }

    public static DateTime GetEndDateTimeValue(string message, DateTime startDateTime)
    {
        DateTime endDateTime = GetDateTimeValue(message);
        while (startDateTime > endDateTime)
        {
            AnsiConsole.MarkupLine("[red]End Date cannot be before Start Date![/]");
            endDateTime = GetDateTimeValue("End Date:");
        }

        return endDateTime;
    }
}
