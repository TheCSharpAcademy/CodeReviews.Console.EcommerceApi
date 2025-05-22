using ExerciseTracker.KamilKolanowski.Enums;
using ExerciseTracker.KamilKolanowski.Models;
using Spectre.Console;

namespace ExerciseTracker.KamilKolanowski.Services;

public class UserInputService
{
    internal Exercise CreateExercise(string exerciseType)
    {
        var name = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the exercise name:").Validate(input =>
                input.Length <= 100
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]Name can't be longer than 100 characters[/]")
            )
        );

        var comment = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the exercise comment:").Validate(input =>
                input.Length <= 200
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]Comment can't be longer than 200 characters[/]")
            )
        );

        var dateStart = AnsiConsole.Prompt(
            new TextPrompt<DateTime>(
                "Enter start date and time [yellow](e.g. 2025-05-16 14:30)[/]:"
            ).Validate(input =>
                input > DateTime.MinValue
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]You provided invalid datetime![/]")
            )
        );

        var dateEnd = AnsiConsole.Prompt(
            new TextPrompt<DateTime>(
                "Enter end date and time [yellow](e.g. 2025-05-16 14:30)[/]:"
            ).Validate(input =>
            {
                if (input <= dateStart)
                    return ValidationResult.Error("[red]End date must be after start date.[/]");
                if (input - dateStart > TimeSpan.FromHours(12))
                    return ValidationResult.Error("[red]You can't work out longer than 12 hours! Get some rest please.[/]");
                return ValidationResult.Success();
            })
        );

        return new Exercise()
        {
            Name = name,
            Comment = comment,
            DateStart = dateStart,
            DateEnd = dateEnd,
            ExerciseType = exerciseType
        };
    }

    internal Exercise EditExercise(Exercise exercise)
    {
        var selectedChoice = AnsiConsole.Prompt(
            new SelectionPrompt<ExerciseTrackerMenu.ColumnsToEdit>()
                .Title("Select what would you like to change")
                .AddChoices(Enum.GetValues<ExerciseTrackerMenu.ColumnsToEdit>())
        );

        switch (selectedChoice)
        {
            case ExerciseTrackerMenu.ColumnsToEdit.Name:
                exercise.Name = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter the exercise name:").Validate(input =>
                        input.Length <= 100
                            ? ValidationResult.Success()
                            : ValidationResult.Error(
                                "[red]Name can't be longer than 100 characters[/]"
                            )
                    )
                );
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
                        if (exercise.DateEnd - input > TimeSpan.FromHours(12))
                            return ValidationResult.Error(
                                "[red]You can't work out longer than 12 hours! Get some rest please.[/]"
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
                    {
                        if (input <= exercise.DateStart)
                            return ValidationResult.Error("[red]End date must be after start date.[/]");
                        if (input - exercise.DateStart > TimeSpan.FromHours(24))
                            return ValidationResult.Error("[red]You can't work out longer than 24 hours![/]");
                        return ValidationResult.Success();
                    })
                );
                break;
            case ExerciseTrackerMenu.ColumnsToEdit.Comment:
                exercise.Comment = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter the exercise comment:").Validate(input =>
                        input.Length <= 200
                            ? ValidationResult.Success()
                            : ValidationResult.Error(
                                "[red]Comment can't be longer than 200 characters[/]"
                            )
                    )
                );
                break;
            case ExerciseTrackerMenu.ColumnsToEdit.ExerciseType:
                var selectedType = GetUserChoiceForExerciseType();

                if (exercise.ExerciseType == selectedType.ToString())
                {
                    AnsiConsole.MarkupLine("[yellow]This exercise already has the selected exercise type assigned.[/]");
                    AnsiConsole.MarkupLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    exercise.ExerciseType = selectedType.ToString();
                }
                break;
        }

        return exercise;
    }

    internal ExerciseType GetUserChoiceForExerciseType()
    {
        var exerciseTypeChoice =  AnsiConsole.Prompt(
            new SelectionPrompt<ExerciseType>()
                .Title("Select which exercise type you would like to use")
                .AddChoices(Enum.GetValues<ExerciseType>()));

        return exerciseTypeChoice;
    }
}
