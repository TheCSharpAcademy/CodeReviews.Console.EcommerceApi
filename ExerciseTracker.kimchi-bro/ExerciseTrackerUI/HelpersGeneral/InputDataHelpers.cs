using Spectre.Console;
using System.Globalization;

internal class InputDataHelpers
{
    public const string DateTimeFormat = "yyyy-MM-dd HH:mm";
    public const string DateFormat = "yyyy-MM-dd";
    public const string TimeFormat = "HH:mm";

    internal static (bool, DateTime, DateTime, TimeSpan, string) GetData()
    {
        DateTime date = GetDate();
        DateTime startTime = GetStartTime(date);
        DateTime endTime = DateTime.Now;
        TimeSpan duration = TimeSpan.Zero;
        string comments = "";

        var choose = DisplayInfoHelpers.GetChoiceFromSelectionPrompt(
            "Select:",
            [
                "Enter exercise end time",
                "Enter exercise duration"
            ]);

        if (choose == DisplayInfoHelpers.Back)
        {
            Console.Clear();
            return (true, startTime, endTime, duration, comments);
        }
        else if (choose == "Enter exercise end time")
        {
            endTime = GetEndTime(date, startTime);
            duration = endTime - startTime;
        }
        else if (choose == "Enter exercise duration")
        {
            duration = GetDuration(startTime);
            endTime = startTime + duration;
        }

        comments = AnsiConsole.Ask<string>("Enter comments:");

        return (false, startTime, endTime, duration, comments);
    }

    private static DateTime GetDate()
    {
        string message = $"Enter exercise's date in \"{DateFormat}\" format or press 'Enter' key for today's date: ";
        AnsiConsole.Markup(message);
        string? dateInput = Console.ReadLine();

        while (true)
        {
            dateInput ??= "";
            bool isValid = DateTime.TryParseExact(dateInput, DateFormat,
                new CultureInfo("en-US"), DateTimeStyles.None, out DateTime date);

            if (dateInput.Trim() == "")
            {
                dateInput = DateTime.Today.ToString(DateFormat);
            }
            else if (!isValid)
            {
                AnsiConsole.Markup($"[red]Invalid input, retry.[/]\n{message}");
                dateInput = Console.ReadLine();
            }
            else
            {
                AnsiConsole.MarkupLine($"Date: [yellow]{date.ToString(DateFormat)}[/]");
                return date;
            }
        }
    }

    private static DateTime GetStartTime(DateTime date)
    {
        string message = $"Enter exercise start time in \"{TimeFormat}\" format: ";
        AnsiConsole.Markup(message);
        string? timeInput = Console.ReadLine();

        while (true)
        {
            timeInput ??= "";
            bool isValid = DateTime.TryParseExact(timeInput, TimeFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out _);

            if (!isValid)
            {
                AnsiConsole.Markup($"[red]Invalid input, retry.[/]\n{message}");
                timeInput = Console.ReadLine();
            }
            else return ConcatenateDateAndTime(date, timeInput);
        }
    }

    private static DateTime ConcatenateDateAndTime(DateTime date, string timeInput)
    {
        DateTime.TryParseExact(timeInput, TimeFormat, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out DateTime time);
        return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
    }

    private static DateTime GetEndTime(DateTime date, DateTime startTime)
    {
        string message = $"Enter exercise end time in \"{TimeFormat}\" format: ";

        AnsiConsole.Markup(message);
        string? timeInput = Console.ReadLine();
        DateTime endTime;

        while (true)
        {
            timeInput ??= "";
            bool isValid = DateTime.TryParseExact(timeInput, TimeFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out _);

            if (isValid)
            {
                endTime = ConcatenateDateAndTime(date, timeInput);

                if (endTime == startTime)
                {
                    AnsiConsole.MarkupLine($"[red]End exercise time can not be the same as the start time.[/]");
                    AnsiConsole.Markup($"{message}");
                    timeInput = Console.ReadLine();
                }
                else if (endTime < startTime)
                {
                    return ConcatenateDateAndTime(date + TimeSpan.FromDays(1), timeInput);
                }
                else return endTime;
            }
            else
            {
                AnsiConsole.Markup($"[red]Invalid input, retry.[/]\n{message}");
                timeInput = Console.ReadLine();
            }
        }
    }

    private static TimeSpan GetDuration(DateTime startTime)
    {
        string message = $"Enter exercise time duration in \"{TimeFormat}\" format: ";
        AnsiConsole.Markup(message);
        string? timeInput = Console.ReadLine();

        while (true)
        {
            timeInput ??= "";
            if (TimeSpan.TryParseExact(timeInput, "hh\\:mm", CultureInfo.InvariantCulture,
                out TimeSpan duration))
            {
                if (duration == TimeSpan.Zero)
                {
                    AnsiConsole.Markup($"[red]Can not enter a duration of 00:00.[/]\n{message}");
                    timeInput = Console.ReadLine();
                }
                else return duration;
            }
            else
            {
                AnsiConsole.Markup($"[red]Invalid input, retry.[/]\n{message}");
                timeInput = Console.ReadLine();
            }
        }
    }
}
