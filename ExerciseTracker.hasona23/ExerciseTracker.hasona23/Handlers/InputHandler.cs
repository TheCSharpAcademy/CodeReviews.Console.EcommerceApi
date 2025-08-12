using System.Globalization;
using ExerciseTracker.hasona23.Models;
using Spectre.Console;

namespace ExerciseTracker.hasona23.Handlers;

public class InputHandler
{
    private const string DateFormat = "dd/MM/yyyy";
    private const string TimeFormat = "HH:mm";
    public Exercise SelectExercise(List<Exercise> exercises)
    {
        var selectionPrompt = new SelectionPrompt<Exercise>()
            .Title("[yellow]Choose an Exercise: [/]")
            .AddChoices(exercises)
            .HighlightStyle(new Style(Color.Yellow));
      
        return AnsiConsole.Prompt(selectionPrompt);
    }

    public ExerciseCreate CreateExercise()
    {
        string? description = GetDescription();
        DateTime day =  GetDay();
        DateTime start = GetStartTime(day);
        DateTime end = GetEndTime(day,start);
        return new ExerciseCreate(description,start,end);
    }

    private DateTime GetStartTime(DateTime day , bool optional = false)
    {
        DateTime start ;
        var prompt =
            new TextPrompt<string?>(
                    $"[yellow]Enter a start time ({TimeFormat}) {(optional ? "or Press enter to skip" : "")}: [/]")
                .PromptStyle(Color.White);
        if (optional)
            prompt.AllowEmpty();
        do
        {
            var startStr =  AnsiConsole.Prompt(prompt);
            if (string.IsNullOrEmpty(startStr) && optional)
            {
                return DateTime.MinValue;
            }
            bool success = DateTime.TryParseExact(startStr, TimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out start);
            if (!success)
            {
                AnsiConsole.MarkupLine($"[red]Wrong Time Format. Check format is {TimeFormat}[/]");
                continue;
            }
            start = new DateTime(day.Year, day.Month, day.Day, start.Hour, start.Minute, 0);
            if (start > DateTime.Now)
            {
                AnsiConsole.MarkupLine("[red]Cant Accept future timings[/]");
            }
        } while (start > DateTime.Now || start == DateTime.MinValue);
        return start;
    }

    private DateTime GetEndTime(DateTime day, DateTime start, bool optional = false)
    {
         DateTime end ;
         var prompt =
             new TextPrompt<string?>(
                     $"[yellow]Enter a End time ({TimeFormat}) {(optional ? "or press enter to skip" : "")}: [/]")
                 .PromptStyle(Color.White);
         if (optional)
             prompt.AllowEmpty();
         
         do
         {
             var endStr =  AnsiConsole.Prompt(prompt);
             if (string.IsNullOrEmpty(endStr) && optional)
             {
                 return DateTime.MinValue;
             }
             bool success = DateTime.TryParseExact(endStr, TimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out end);
             if (!success)
             {
                 AnsiConsole.MarkupLine($"[red]Wrong Time Format. Check format is {TimeFormat}[/]");
                 continue;
             }
             end = new DateTime(day.Year, day.Month, day.Day, end.Hour, end.Minute, 0);
             if (end <= start)
             {
                 end = end.AddDays(1);
             }
             if (end > DateTime.Now)
             {
                 AnsiConsole.MarkupLine("[red]Cant Accept future timings[/]");
             }

         } while (end > DateTime.Now || end == DateTime.MinValue);
         return end;
    }

    private DateTime GetDay(bool optional = false)
    {
        DateTime day;
        do
        {
            var dayStr =  AnsiConsole.Prompt(new TextPrompt<string?>($"[yellow]Enter a date ({DateFormat}) or press enter {(optional?"To skip":"to get current date")}: [/]").PromptStyle(Color.White).AllowEmpty());
            if (string.IsNullOrEmpty(dayStr) && optional)
            {
                return DateTime.MinValue;
            }
            if (string.IsNullOrEmpty(dayStr))
            {
                day = DateTime.Today;
                break;
            }
            bool success = DateTime.TryParseExact(dayStr, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out day);
            if (!success)
            {
                AnsiConsole.MarkupLine($"[red]Wrong date format. Check format is ({DateFormat})[/]");

            }
            if (day > DateTime.Today)
            {
                AnsiConsole.MarkupLine("[red]Cant Accept Future date[/]");
            }
        }while(day == DateTime.MinValue || day > DateTime.Today);

        return day;
    }

    private string? GetDescription()
    {
        return AnsiConsole.Prompt(new TextPrompt<string?>("[yellow]Enter Description or press enter to skip: [/]").AllowEmpty().PromptStyle(new Style(Color.White)));
    }

    public ExerciseUpdate UpdateExercise(List<Exercise> exercises)
    {
        int exerciseId = SelectExercise(exercises).Id;
        DateTime day = GetDay(true);
        DateTime start = GetStartTime(day,true);
        DateTime end = GetEndTime(day,start,true);
        return new ExerciseUpdate(exerciseId,GetDescription(),start,end);
    }
}