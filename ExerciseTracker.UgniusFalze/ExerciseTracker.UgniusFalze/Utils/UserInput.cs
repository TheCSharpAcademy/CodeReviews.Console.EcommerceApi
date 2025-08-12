using System.Text.RegularExpressions;
using ExerciseTracker.UgniusFalze.Models;
using Spectre.Console;

namespace ExerciseTracker.UgniusFalze.Utils;

public enum InitialMenuOptions
{
    ViewExercises,
    ManageExercises,
    AddExercise,
    Exit
}

public enum ManageMenuOptions
{
    UpdateComment,
    UpdateStartDate,
    UpdateEndDate,
    UpdateRepetitions,
    Delete,
    Exit
}

public static partial class UserInput
{
    public static InitialMenuOptions DisplayInitialMenu()
    {
        var selectionPrompt = new SelectionPrompt<InitialMenuOptions>()
            .AddChoices(
                InitialMenuOptions.ViewExercises,
                InitialMenuOptions.ManageExercises,
                InitialMenuOptions.AddExercise,
                InitialMenuOptions.Exit);
        AddConverter(selectionPrompt);
        var choice = AnsiConsole.Prompt(selectionPrompt);
        return choice;
    }

    private static void AddConverter<T>(SelectionPrompt<T> selectionPrompt) where T : System.Enum
    {
        selectionPrompt.Converter = arg => EnumRegex().Replace(arg.ToString(), " $1");
    }

public static Pullup? GetExerciseToManage(List<Pullup> exercises)
    {
        if (exercises.Count == 0)
        {
            Display.EmptyExercises();
            Display.Continue();
            return null;
        }
        var selectionPrompt = new SelectionPrompt<Pullup>();
        selectionPrompt.Title("Select which exercise you want to manage");
        selectionPrompt.Converter = pullup => $"{pullup.PullupId} {pullup.Duration.TotalMinutes} {pullup.Comment}";
        foreach (var exercise in exercises)
        {
            selectionPrompt.AddChoice(exercise);
        }

        var choice = AnsiConsole.Prompt(selectionPrompt);
        return choice;
    }

    private static T GetInput<T>(string message)
    {
        var textPrompt = AnsiConsole.Ask<T>(message);
        return textPrompt;
    }
    
    public static string GetCommentInput()
    {
        return GetInput<string>("Please enter the comment for your exercise: ");
    }
    
    public static int GetRepetitionInput()
    {
        var prompt = new TextPrompt<int>("Please enter your total repetitions for this exercise: ");
        prompt.Validate(repetition => repetition < 0 ? ValidationResult.Error("Repetitions must be greater or equal to zero.") : ValidationResult.Success());
        return AnsiConsole.Prompt(prompt);
    }

    public static ManageMenuOptions DisplayManageMenu()
    {
        var selectionPrompt = new SelectionPrompt<ManageMenuOptions>()
            .AddChoices(
                ManageMenuOptions.UpdateComment,
                ManageMenuOptions.UpdateStartDate,
                ManageMenuOptions.UpdateEndDate,
                ManageMenuOptions.UpdateRepetitions,
                ManageMenuOptions.Delete,
                ManageMenuOptions.Exit
                );
        AddConverter(selectionPrompt);
        var choice = AnsiConsole.Prompt(selectionPrompt);
        return choice;
    }

    public static DateTime GetStartDate(DateTime? endDate = null)
    {
        var textPrompt = new TextPrompt<DateTime>("Enter the starting date for the exercise")
            .ValidationErrorMessage(Display.InvalidDate());
        if (endDate != null)
        {
            textPrompt.Validate(
                startDate =>
                {
                    if (endDate > startDate)
                    {
                        return ValidationResult.Success();
                    }
                    else
                    {
                        return ValidationResult.Error(
                            $"Start date should be lower than the end date, end date is {endDate.ToString()}");
                    }
                });
        }

        return AnsiConsole.Prompt(textPrompt);
    }
    
    public static DateTime GetEndDate(DateTime? startDate = null)
    {
        var textPrompt = new TextPrompt<DateTime>("Enter the starting date for the exercise")
            .ValidationErrorMessage(Display.InvalidDate());
        if (startDate != null)
        {
            textPrompt.Validate(
                endDate =>
                {
                    if (endDate > startDate)
                    {
                        return ValidationResult.Success();
                    }
                    else
                    {
                        return ValidationResult.Error(
                            $"End date should be lower than the start date, start date is {startDate.ToString()}");
                    }
                });
        }

        return AnsiConsole.Prompt(textPrompt);
    }

    [GeneratedRegex("(\\B[A-Z])")]
    private static partial Regex EnumRegex();
}