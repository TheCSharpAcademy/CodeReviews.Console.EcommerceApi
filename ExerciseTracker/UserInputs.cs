using ExerciseTracker.Models;
using Spectre.Console;
using System.Globalization;

namespace ExerciseTracker;

class UserInputs
{
    internal static string GetComment(IExercices? exercice)
    {
        if(exercice != null) AnsiConsole.MarkupLine($"[Blue]Current[/] comment:\n{exercice.Comment}\n");
        string comment = AnsiConsole.Ask<string>("Write a [Green]new[/] comment: ");
        return comment;
    }

    internal static DateTime GetDateTime()
    {
        DateTime returnedDT;
        string date = AnsiConsole.Prompt(new TextPrompt<string>("Enter a [Green]new[/] date(format = yyyy.MM.dd): ").Validate(x => DateTime.TryParseExact(x, "yyyy.MM.dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out returnedDT)));
        string time = AnsiConsole.Prompt(new TextPrompt<string>("Enter a [Green]new[/] time(format= HH:mm:ss): ").Validate(x => DateTime.TryParseExact(x, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out returnedDT)));
        return returnedDT = DateTime.Parse($"{date} {time}");
    }

    internal static IExercices ChoooseExercice(IExercices[] exercices)
    {
        return AnsiConsole.Prompt(new SelectionPrompt<IExercices>().Title("Choose an exercice below:").AddChoices(exercices)
            .UseConverter(x => $"{x.Id} - {x.Start} - {x.End}"));
    }

    internal static string ChooseUpdateOption()
    {
        string option = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Choose an option below:")
            .AddChoices("Start", "End", "Comment", "Cancel"));
        return option;
    }

    internal static IExercices UpdateStart(IExercices exercice)
    {
        DateTime newStart = new();
        bool valid = false;
        AnsiConsole.MarkupLine($"[Blue]Current[/] start: {exercice.Start.ToString("yyyy.MM.dd HH.mm.ss")}");
        while (!valid)
        {
            newStart = GetDateTime();
            valid = newStart < exercice.End;
            if (!valid) AnsiConsole.MarkupLine($"The start must be [red]before[/] the end({exercice.End.ToString("yyyy.MM.dd HH:mm:ss")})");
        }
        exercice.Start = newStart;
        return exercice;
    }

    internal static IExercices UpdateEnd(IExercices exercice)
    {
        DateTime newEnd = new();
        bool valid = false;
        AnsiConsole.MarkupLine($"[Blue]Current[/] end: {exercice.Start.ToString("yyyy.MM.dd HH.mm.ss")}");
        while (!valid)
        {
            newEnd = GetDateTime();
            valid = exercice.Start < newEnd;
            if (!valid) AnsiConsole.MarkupLine($"The end must be [red]after[/] the start({exercice.Start.ToString("yyyy.MM.dd HH:mm:ss")})");
        }
        exercice.End = newEnd;
        return exercice;
    }

    internal static bool Validation(string message)
    {
        return AnsiConsole.Confirm(message, false);
    }
}