using System.Text.RegularExpressions;
using Data.Entities;
using Spectre.Console;

namespace ExerciseTracker;

public static class UI
{
    public static void Clear() => AnsiConsole.Clear();

    public static string MenuSelection(string title, string[] options)
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                .AddChoices(options)
        );
    }

    public static string StringResponse(string question) => AnsiConsole.Ask<string>(question + ":");

    public static string StringResponseWithFormat(string prompt, string format) {
        while (true)
        {
            string response = AnsiConsole.Ask<string>(prompt + $"[grey] formatted as '{format}[/]':");

            if (Regex.IsMatch(response, format))
            {
                return response;
            }

            AnsiConsole.MarkupLine($"Requires format '[yellow]{format}[/]'");
        } 
    }

    public static TimeOnly TimeOnlyResponse(string prompt) 
    {
        while (true)
        {
            var response = AnsiConsole.Ask<string>(prompt + ":");

            if (TimeOnly.TryParseExact(response, "HH:mm", out TimeOnly time)) {
                return time;
            }

            AnsiConsole.MarkupLine("[red]Invalid time.[/] [grey]Format as 'HH:mm'[/]");
        }
    } 

    public static string StringResponseWithDefault(string question, string defaultResponse)
    {
        var response = AnsiConsole.Prompt(
            new TextPrompt<string>(question + " [grey](Press 'enter' to leave as '" + defaultResponse + "'[/]):")
                .AllowEmpty()
        );

        if (response == null || response == "")
        {
            return defaultResponse;
        }
        else
        {
            return response;
        }
    }

    public static int IntResponse(string question)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>(question + ":")
                .ValidationErrorMessage("[red]That's not a valid number[/]")
                .Validate(id =>
                {
                    return id switch
                    {
                        < 0 => ValidationResult.Error("[red]Id's must be a number greater than or equal to 0[/]"),
                        _ => ValidationResult.Success(),
                    };
                })
        );
    }

    public static string TimeResponse(string question)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>(question + " formatted like [yellow]23:59 dd-MM-yy[/]:")
                .PromptStyle("green")
                .ValidationErrorMessage("format times like [red]23:59 dd-MM-yy[/]")
                .Validate(time =>
                {
                    return DateTime.TryParseExact(time, "HH:mm dd-MM-yy", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime);
                })
        );
    }

    public static DateTime DateTimeResponse(string prompt)
    {
        while (true)
        {
            AnsiConsole.Markup(prompt + $". [grey]Format as 'yyyy-MM-dd HH:mm:ss'[/]: ");
            var response = Console.ReadLine();

            if (DateTime.TryParse(response, out DateTime dt)) {
                return dt;
            }

            AnsiConsole.MarkupLine($"[red]Invalid date time.[/] [grey]Format as {DateTime.Today}[/]: ");
        }
    }

    public static DateTime DateTimeResponseWithDefault(string prompt, DateTime defaultDateTime)
    {
        while (true)
        {
            AnsiConsole.Markup(prompt + $". [grey]Format as 'yyyy-MM-dd HH:mm:ss'[/]. Press 'enter' to leave as {defaultDateTime}: ");
            var response = Console.ReadLine();
            if (response == "") {
                return defaultDateTime;
            }
            if (DateTime.TryParse(response, out DateTime dt)) {
                return dt;
            }

            AnsiConsole.MarkupLine($"[red]Invalid date time.[/] [grey]Format as {DateTime.Today}[/]");
        }
    }

    public static void ConfirmationMessage(string message)
    {
        if (message != "") {
            AnsiConsole.MarkupLine($"{message}. Press 'enter' to continue");
        } else {
            AnsiConsole.Console.MarkupLine("Press 'enter' to continue");
        }
        Console.ReadLine();
        AnsiConsole.Clear();
    }

    public static void InvalidationMessage(string message) => AnsiConsole.MarkupLine("[red]" + message + "[/]");

    public static void DisplayExercises(IEnumerable<Exercise> exercises) {
        var table = new Table();

        string[] columns = ["ID", "Start Date", "End Date", "Duration", "Comments"];
        table.AddColumns(columns);

        foreach(var exercise in exercises)
        {
            table.AddRow(
                exercise.Id.ToString(),
                exercise.DateStart.ToString(),
                exercise.DateEnd.ToString(),
                exercise.Duration.ToString(),
                exercise.Comments ?? ""
            );
        }
        AnsiConsole.Write(table);
    }
}
