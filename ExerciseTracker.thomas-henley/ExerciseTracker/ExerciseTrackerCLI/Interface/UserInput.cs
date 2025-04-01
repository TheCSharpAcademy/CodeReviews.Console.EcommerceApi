using System.Globalization;
using ExerciseTrackerCLI.Models;
using Spectre.Console;

namespace ExerciseTrackerCLI.Interface;

public static class UserInput
{
    public static string MainMenuChoice()
    {
        var prompt = new SelectionPrompt<string>()
            .Title("[white]What would you like to do?[/]")
            .AddChoices("Add a run", "Review runs", "Exit");
        
        return AnsiConsole.Prompt(prompt);
    }

    public static void WelcomeMessage()
    {
        AnsiConsole.MarkupLine("[white]Welcome to the Run Tracker![/]");
    }

    public static void Goodbye()
    {
        AnsiConsole.MarkupLine("[white]Thank you for using the Run Tracker.[/]");
    }

    public static void PrintLine(string message = "")
    {
        AnsiConsole.MarkupLine(message);
    }

    public static DateTime GetStartTime()
    {
        return GetStartTime(DateTime.Now);
    }

    public static DateTime GetStartTime(DateTime dtDefault)
    {
        var prompt = new TextPrompt<string>($"[white]Please enter the start time ({Validators.DtFormat}), or enter to accept the default[/]")
            .DefaultValue(dtDefault.ToString(Validators.DtFormat))
            .Validate(Validators.DateTimeFormatValidator);
        
        return DateTime.ParseExact(AnsiConsole.Prompt(prompt), Validators.DtFormat, new CultureInfo("en-US"));
    }

    public static DateTime GetEndTime(DateTime start)
    {
        return GetEndTime(DateTime.Now, start);
    }

    public static DateTime GetEndTime(DateTime dtDefault, DateTime start)
    {
        var prompt = new TextPrompt<string>($"[white]Please enter the end time ({Validators.DtFormat}), or enter to accept the default[/]")
            .DefaultValue(dtDefault.ToString(Validators.DtFormat))
            .Validate(s => Validators.EndTimeValidator(s, start));
        
        return DateTime.ParseExact(AnsiConsole.Prompt(prompt), Validators.DtFormat, new CultureInfo("en-US"));
    }

    public static string GetComments(string placeholder = "")
    {
        var prompt = new TextPrompt<string>("[white]Enter comments (or press enter to accept default)[/]")
            .DefaultValue(placeholder)
            .AllowEmpty();
        
        return AnsiConsole.Prompt(prompt);
    }

    public static TreadmillRun RunListMenu(IEnumerable<TreadmillRun> runs)
    {
        var prompt = new SelectionPrompt<TreadmillRun>()
            .Title("[white]Select a record to manage:[/]")
            .AddChoices(runs)
            .UseConverter(run => run.ListDisplay());
        
        return AnsiConsole.Prompt(prompt);
    }

    public static object ManageRunMenu(TreadmillRun run)
    {
        AnsiConsole.MarkupLine("[white]Edit run:[/]");
        var prompt = new SelectionPrompt<string>()
            .Title("[green]" + run.LongDisplay() + "[/]")
            .AddChoices("Edit Start Time", "Edit End Time", "Edit Comment", "Delete Run", "Go Back");
        
        return AnsiConsole.Prompt(prompt);
    }
}