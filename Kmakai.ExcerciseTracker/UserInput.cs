using Kmakai.ExerciseTracker.Models;
using Spectre.Console;

namespace Kmakai.ExerciseTracker;

public class UserInput
{
    public static DateTime GetDateAndTime()
    {
        DateTime date;

        while (true)
        {
            var input = AnsiConsole.Ask<string>("Enter the date and time in format mm/dd/yyyy hh:mm => ");

            if (DateTime.TryParse(input, out date) && input.Trim().Split(" ").Length >= 2)
            {
                return date;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid date and time format. Please try again.[/]");
            }
        }

    }

    public static string? GetComment()
    {
        var input = AnsiConsole.Ask<string>("Enter any comments (optional) => ", "n/a");

        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }
        else
        {
            return input;
        }
    }

    public static int GetId(List<Exercise> exercises)
    {
        while (true)
        {
            var input = AnsiConsole.Ask<string>("Enter the id of the exercise you want to update => ");

            if (int.TryParse(input, out int id) && exercises.Any(x => x.Id == id))
            {
                return id;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid id. Please try again.[/]");
            }
        }

    }

    internal static int GetChoice()
    {
        var choice = AnsiConsole.Prompt(
                     new SelectionPrompt<string>()
                     .Title("Select an option")
                     .PageSize(10)
                     .AddChoices(new[]
                     {
                         "1. Add an exercise" ,
                         "2. Update an exercise",
                         "3. Delete an exercise",
                         "4. View all exercises",
                         "0. Exit"
                     }));

        return choice switch
        {
            "1. Add an exercise" => 1,
            "2. Update an exercise" => 2,
            "3. Delete an exercise" => 3,
            "4. View all exercises" => 4,
            "0. Exit" => 0,

        };
    }
}

