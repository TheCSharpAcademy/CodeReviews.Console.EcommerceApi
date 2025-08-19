using ExerciseTracker.jollejonas.Enums;
using ExerciseTracker.jollejonas.Models;
using Spectre.Console;


namespace ExerciseTracker.jollejonas.UserInput;

public class UserInput : IUserInput
{
    public DateTime GetDateTime()
    {

        string dateTime = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter date and time in 'yyyy-MM-dd HH:mm': ")
                .PromptStyle("bold yellow")
                .ValidationErrorMessage("[red]Invalid date and time[/]")
                .Validate(input => Validation.Validation.ValidateDate(input))
            );

        return DateTime.Parse(dateTime);
    }

    public MenuOptions GetMenuOption()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<MenuOptions>()
                .Title("Select an option")
                .AddChoices(Enum.GetValues<MenuOptions>())
                );
    }
    public string GetExerciseComments()
    {
        string exerciseComments = AnsiConsole.Prompt(
            new TextPrompt<string>("[bold yellow]Enter comments for the exercise:[/]")
            .ValidationErrorMessage("[red] Your comment has to be between 1 and 1500 characters")
            .Validate(input => Validation.Validation.ValidateComment(input)));
        return exerciseComments;
    }

    public Exercise GetExercise(List<Exercise> exercises)
    {
        var cancelOption = new Exercise
        {
            DateStart = DateTime.MinValue,
            DateEnd = DateTime.MinValue,
            Duration = TimeSpan.Zero,
            Comments = "Cancel"
        };

        exercises.Add(cancelOption);

        var selectedExercise = AnsiConsole.Prompt(
            new SelectionPrompt<Exercise>()
                .Title("Select an exercise")
                .AddChoices(exercises)
                .UseConverter(exercises => $"{exercises.DateStart} - {exercises.DateEnd} - {exercises.Duration} - {exercises.Comments}")
                );

        if (selectedExercise == cancelOption)
        {
            Console.WriteLine("Canceled");
            return null;
        }

        return selectedExercise;
    }

    public bool GetConfirmation(string message)
    {
        return AnsiConsole.Confirm(message);
    }
}