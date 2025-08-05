using ExerciseTracker.Niasua.Models;
using Spectre.Console;

namespace ExerciseTracker.Niasua.UserInput;

public static class UserInputHandler
{
    public static Exercise GetExerciseInput()
    {
        Console.WriteLine("Enter exercise start date and time (yyyy-mm-dd hh:mm)");
        DateTime start = ReadDateTime();

        Console.WriteLine("Enter exercise start date and time (yyyy-mm-dd hh:mm)");
        DateTime end = ReadDateTime();

        Console.WriteLine("Enter any comments: ");
        string? comments = Console.ReadLine();

        return new Exercise
        {
            DateStart = start,
            DateEnd = end,
            Comments = comments
        };
    }

    private static DateTime ReadDateTime()
    {
        while (true)
        {
            var input = Console.ReadLine();

            if (DateTime.TryParse(input, out var result))
                return result;

            Console.WriteLine("Invalid format. Please use format like: yyyy-mm-dd hh:mm");
        }
    }

    public static int? GetId()
    {
        while (true)
        {
            var idInput = AnsiConsole.Ask<string>("Type Exercise's ID (0 to cancel)");

            if (idInput == "0")
            {
                return null;
            }

            AnsiConsole.MarkupLine("[red]Invalid ID. Please enter a positive number or 0 to cancel.[/]");
        }
    }
}
