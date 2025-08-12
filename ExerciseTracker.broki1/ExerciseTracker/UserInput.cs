using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace ExerciseTracker;

internal class UserInput
{
    internal char GetMainMenuInput()
    {
        return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("MAIN MENU")
                .AddChoices("1. Add exercise",
                "2. View exercises",
                "3. Update exercise",
                "4. Delete exercise",
                "5. Exit application")
                )[0];
    }

    internal DateOnly GetDate(string startOrEnd)
    {
        var dateString = AnsiConsole.Prompt(
            new TextPrompt<string>($"[green]Enter exercise {startOrEnd} date (MM-DD-YY):[/]")
            .PromptStyle("purple")
            .ValidationErrorMessage("\n[red]INVALID DATE. MUST BE IN MM-DD-YY FORMAT.[/]\n")
            .Validate(date => DateOnly.TryParseExact(date, "MM-dd-yy", out _))
            );

        var date = DateOnly.ParseExact(dateString, "MM-dd-yy");

        return date;
    }

    internal TimeOnly GetTime(string startOrEnd)
    {
        var timeString = AnsiConsole.Prompt(
            new TextPrompt<string>($"[green]Enter exercise {startOrEnd} time (HH:MM AM/PM):[/]")
            .PromptStyle("purple")
            .ValidationErrorMessage("\n[red]INVALID TIME. MUST BE IN HH:MM AM/PM FORMAT.[/]\n")
            .Validate(time => TimeOnly.TryParseExact(time, "hh:mm tt", out _))
            );

        var time = TimeOnly.ParseExact(timeString, "hh:mm tt");
        
        return time;
    }

    internal string GetComments()
    {
        var comments = AnsiConsole.Prompt(
            new TextPrompt<string>($"[green]Enter exercise comments:[/]")
            .PromptStyle("purple")
            .ValidationErrorMessage("\n[red]CANNOT BE EMPTY. ENTER EXERCISE COMMENTS.[/]\n")
            .Validate(str => !str.IsNullOrEmpty()));

        return comments;
    }
}
