using Spectre.Console;

namespace ExerciseTracker.selnoom.Helpers;

class Validation
{
    public static string? ValidateTimeInput()
    {
        string timeInput = AnsiConsole.Ask<string>("Enter date (yyyy-MM-dd HH:mm) or 0 to return to menu:");

        if (timeInput == "0")
        {
            return null;
        }

        DateTime parsedDate;
        while (!DateTime.TryParseExact(timeInput, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out parsedDate))
        {
            AnsiConsole.MarkupLine("[bold red]Invalid input. Please try again.[/]\n");
            timeInput = AnsiConsole.Ask<string>("Enter date (yyyy-MM-dd HH:mm) or 0 to return to menu:");

            if (timeInput == "0")
            {
                return null;
            }
        }

        return parsedDate.ToString("yyyy-MM-dd HH:mm");
    }

    internal static string? ValidateEndTimeInput(string? startTime)
    {
        if (startTime == null)
        {
            return null;
        }

        while (true)
        {
            string? endTime = ValidateTimeInput();

            if (endTime == null)
            {
                return null;
            }

            if (!DateTime.TryParseExact(startTime, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime parsedStart))
            {
                AnsiConsole.MarkupLine("[bold red]Invalid start time format.[/]\n");
                return null;
            }

            if (!DateTime.TryParseExact(endTime, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime parsedEnd))
            {
                AnsiConsole.MarkupLine("[bold red]Invalid end time format. Please try again.[/]\n");
                continue;
            }

            if (parsedEnd < parsedStart)
            {
                AnsiConsole.MarkupLine("[bold red]The end time cannot be before the start time. Please try again.[/]\n");
            }
            else
            {
                return endTime;
            }
        }
    }

    public static double ValidatePositiveDouble(string prompt)
    {
        double value;
        while (true)
        {
            var input = AnsiConsole.Ask<string>(prompt);
            if (double.TryParse(input, out value) && value >= 0)
            {
                return value;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Please enter a positive number or 0 to return to the menu:[/]");
            }
        }
    }

    public static int ValidatePositiveInt(string prompt)
    {
        int value;
        while (true)
        {
            var input = AnsiConsole.Ask<string>(prompt);
            if (int.TryParse(input, out value) && value >= 0)
            {
                return value;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Please enter positive integer or 0 to return:[/]");
            }
        }
    }
}
