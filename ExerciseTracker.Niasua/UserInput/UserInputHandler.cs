using ExerciseTracker.Niasua.Models;
using Spectre.Console;

namespace ExerciseTracker.Niasua.UserInput;

public static class UserInputHandler
{
    public static Exercise? GetExerciseInput()
    {
        Console.WriteLine("Enter exercise start date and time (dd/mm/yyyy hh:mm) (0 to leave):");
        DateTime? start = ReadDateTime();
        if (start == null)
            return null;

        Console.WriteLine("Enter exercise end date and time (dd/mm/yyyy hh:mm) (0 to leave):");
        DateTime? end = ReadDateTime();
        if (end == null)
            return null;

        Console.WriteLine("Enter any comments:");
        string? comments = Console.ReadLine();

        return new Exercise
        {
            DateStart = start.Value,
            DateEnd = end.Value,
            Comments = comments,
            Duration = end.Value - start.Value
        };
    }


    public static DateTime? ReadDateTime()
    {
        string input = Console.ReadLine()?.Trim() ?? "";

        if (input == "0" || string.IsNullOrWhiteSpace(input))
            return null;

        if (DateTime.TryParse(input, out DateTime result))
            return result;

        Console.WriteLine("Invalid date format. Try again.");
        return ReadDateTime();
    }

    public static int? GetId()
    {
        while (true)
        {
            var idInput = AnsiConsole.Ask<string>("Type Exercise's ID (0 to cancel):");

            if (!int.TryParse(idInput, out int id))
            {
                AnsiConsole.MarkupLine("[red]Invalid input. Please enter a number.[/]");
            }
            else if (id == 0)
            {
                return null;
            }
            else if (id > 0)
            {
                return id;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]ID must be a positive number or 0 to cancel.[/]");
            }


            AnsiConsole.MarkupLine("[red]Invalid ID. Please enter a positive number or 0 to cancel.[/]");
        }
    }

    public static DateTime? GetDateTime(string prompt)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>(prompt)
                .AllowEmpty()
                .PromptStyle("green"));

        input = input.Trim();

        if (string.IsNullOrWhiteSpace(input) || input == "0")
            return null;

        if (DateTime.TryParse(input, out var date))
            return date;

        AnsiConsole.MarkupLine("[red]Invalid date format.[/]");
        return GetDateTime(prompt);
    }

}
