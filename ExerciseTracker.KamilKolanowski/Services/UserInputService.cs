using ExerciseTracker.KamilKolanowski.Enums;
using ExerciseTracker.KamilKolanowski.Models;
using Spectre.Console;

namespace ExerciseTracker.KamilKolanowski.Services;

public class UserInputService
{
    internal Exercise CreateExercise()
    {
        var name = AnsiConsole.Ask<string>("Enter the exercise name:");
        var comment = AnsiConsole.Ask<string>("Enter the exercise comment:");

        var dateStart = AnsiConsole.Prompt(
            new TextPrompt<DateTime>(
                    "Enter start date and time [yellow](e.g. 2025-05-16 14:30)[/]:"
                )
                .Validate(input =>
                    input > DateTime.MinValue
                        ? ValidationResult.Success()
                        : ValidationResult.Error("You provided invalid datetime!")
            )
        );
        
        var dateEnd = AnsiConsole.Prompt(
            new TextPrompt<DateTime>(
                "Enter end date and time [yellow](e.g. 2025-05-16 14:30)[/]:"
            )
            .Validate(input =>
                input > dateStart
                    ? ValidationResult.Success()
                    : ValidationResult.Error(
                        "[red]Invalid date (end date must be greater than start date).[/]"
                    )
            )
        );

        return new Exercise()
        {
            Name = name,
            Comment = comment,
            DateStart = dateStart,
            DateEnd = dateEnd,
        };
    }

    internal Exercise EditExercise(Exercise exercise)
    {
        var selectedChoice = AnsiConsole.Prompt(
            new SelectionPrompt<ExerciseTrackerMenu.ColumnsToEdit>()
                .Title("Select what would you like to do")
                .AddChoices(Enum.GetValues<ExerciseTrackerMenu.ColumnsToEdit>())
        );

        switch (selectedChoice)
        {
            case ExerciseTrackerMenu.ColumnsToEdit.Name:
                exercise.Name = AnsiConsole.Ask<string>("Enter the exercise name:");
                break;
            case ExerciseTrackerMenu.ColumnsToEdit.DateStart:
                exercise.DateStart = AnsiConsole.Prompt(
                    new TextPrompt<DateTime>(
                        "Enter start date and time [yellow](e.g. 2025-05-16 14:30)[/]:"
                    ).Validate(input =>
                    {
                        if (exercise.DateEnd <= input)
                            return ValidationResult.Error(
                                "[red]Start time must be before end time.[/]"
                            );
                        if (exercise.DateEnd - input > TimeSpan.FromHours(24))
                            return ValidationResult.Error(
                                "[red]You can't work out longer than 24 hours![/]"
                            );
                        return ValidationResult.Success();
                    })
                );
                break;
            case ExerciseTrackerMenu.ColumnsToEdit.DateEnd:
                exercise.DateEnd = AnsiConsole.Prompt(
                    new TextPrompt<DateTime>(
                        "Enter end date and time [yellow](e.g. 2025-05-16 14:30)[/]:"
                    ).Validate(input =>
                        input > exercise.DateStart
                            ? ValidationResult.Success()
                            : ValidationResult.Error(
                                "[red]Invalid date (end date must be greater than start date).[/]"
                            )
                    )
                );
                break;
            case ExerciseTrackerMenu.ColumnsToEdit.Comment:
                exercise.Comment = AnsiConsole.Ask<string>("Enter the comment for exercise:");
                break;
        }

        return exercise;
    }
}
