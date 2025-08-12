using ExerciseTracker.Dejmenek.Enums;
using ExerciseTracker.Dejmenek.Models;
using Spectre.Console;

namespace ExerciseTracker.Dejmenek.Services;
public class UserInteractionService : IUserInteractionService
{
    public string GetDateTime()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("Enter date time in 'yyyy-MM-dd HH:mm format': ")
                .PromptStyle("green")
                .ValidationErrorMessage("[red]That is not a valid date time format.[/]")
                .Validate(Validation.IsValidDateTimeFormat)
            );
    }

    public string GetComment()
    {
        return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your comment for the exercise: ")
                );
    }

    public MenuOptions GetMenuOption()
    {
        return AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                    .Title("What would you like to do?")
                    .AddChoices(Enum.GetValues<MenuOptions>())
            );
    }

    public Exercise GetExercise(List<Exercise> exercises)
    {
        return AnsiConsole.Prompt(
                new SelectionPrompt<Exercise>()
                    .Title("Select exercise")
                    .UseConverter(exercise => $"On {exercise.StartTime:yyyy-MM-dd HH:mm} for {exercise.Duration} minutes - {exercise.Comments}")
                    .AddChoices(exercises)
            );
    }

    public void GetUserInputToContinue()
    {
        AnsiConsole.MarkupLine("Press Enter to continue...");
        Console.ReadLine();
    }

    public bool GetConfirmation(string title)
    {
        return AnsiConsole.Confirm(title);
    }
}
