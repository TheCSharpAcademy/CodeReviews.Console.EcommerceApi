using ExerciseTracker.Niasua.Models;
using Spectre.Console;

namespace ExerciseTracker.Niasua.UserInput;

public static class UserInputHandler
{
    public static Exercise GetExerciseInput()
    {
        Console.WriteLine("Enter exercise start date and time (dd/mm/yyyy hh:mm):");
        DateTime start = ReadDateTime();

        Console.WriteLine("Enter exercise end date and time (dd/mm/yyyy hh:mm):");
        DateTime end = ReadDateTime();

        Console.WriteLine("Enter any comments: ");
        string? comments = Console.ReadLine();

        return new Exercise
        {
            DateStart = start,
            DateEnd = end,
            Comments = comments,
            Duration = end - start
        };
    }

    private static DateTime ReadDateTime()
    {
        while (true)
        {
            var input = Console.ReadLine();

            if (DateTime.TryParse(input, out var result))
                return result;

            Console.WriteLine("Invalid format. Please use format like: (dd/mm/yyyy hh:mm)");
        }
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
        
        if (string.IsNullOrWhiteSpace(input)) return null;

        if (DateTime.TryParse(input, out var date))
            return date;

        AnsiConsole.MarkupLine("[red]Invalid date format.[/]");
        return GetDateTime(prompt);
    }
}
