using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using Exercise_Tracker.Models;
using Exercise_Tracker.Views;
using Spectre.Console;

namespace Exercise_Tracker.Utils;

public static class Helpers
{
    public static string? GetDisplayName(this Enum enumValue)
    {
        return enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .First()
            ?.GetCustomAttribute<DisplayAttribute>()
            ?.Name;
    }

    public static int GetExerciseId(List<Exercise> exercises)
    {
        UserInterface.ShowAllExercises(exercises);

        if (exercises.Count <= 0)
        {
            AnsiConsole.MarkupLine("[red]Please create an exercise to continue.[/]");
        }
        else
        {
            var exerciseArray = exercises.Select(e => e.Id).ToArray();

            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("Select an exercise to view details:")
                    .AddChoices(exerciseArray)
            );

            return userChoice;
        }

        return 0;
    }

    internal static DateTime[] GetDates()
    {
        var startDateInput = AnsiConsole.Ask<string>(
            "Enter Exercise Start Time (yyyy-MM-dd HH:mm): "
        );

        while (!Validator.IsValidDate(startDateInput, "yyyy-MM-dd HH:mm"))
            startDateInput = AnsiConsole.Ask<string>(
                "\n[red]Invalid date. Format: yyyy-MM-dd HH:mm. Please try again:[/]\n"
            );

        var endDateInput = AnsiConsole.Ask<string>("Enter Exercise End Time (yyyy-MM-dd HH:mm): ");

        while (!Validator.IsValidDate(endDateInput, "yyyy-MM-dd HH:mm"))
            endDateInput = AnsiConsole.Ask<string>(
                "\n[red]Invalid date. Format: yyyy-MM-dd HH:mm. Please try again:[/]\n"
            );

        while (!Validator.IsStartDateBeforeEndDate(startDateInput, endDateInput))
        {
            AnsiConsole.MarkupLine(
                "\n[red]Start date must be before end date. Please try again:[/]"
            );
            startDateInput = AnsiConsole.Ask<string>(
                "Enter Exercise Start Time (yyyy-MM-dd HH:mm): "
            );

            while (!Validator.IsValidDate(startDateInput, "yyyy-MM-dd HH:mm"))
                startDateInput = AnsiConsole.Ask<string>(
                    "\n[red]Invalid date. Format: yyyy-MM-dd HH:mm. Please try again:[/]\n"
                );

            endDateInput = AnsiConsole.Ask<string>("Enter Exercise End Time (yyyy-MM-dd HH:mm): ");

            while (!Validator.IsValidDate(endDateInput, "yyyy-MM-dd HH:mm"))
                endDateInput = AnsiConsole.Ask<string>(
                    "\n[red]Invalid date. Format: yyyy-MM-dd HH:mm. Please try again:[/]\n"
                );
        }

        var startDate = DateTime.ParseExact(
            startDateInput,
            "yyyy-MM-dd HH:mm",
            CultureInfo.InvariantCulture
        );
        var endDate = DateTime.ParseExact(
            endDateInput,
            "yyyy-MM-dd HH:mm",
            CultureInfo.InvariantCulture
        );
        return [startDate, endDate];
    }
}
