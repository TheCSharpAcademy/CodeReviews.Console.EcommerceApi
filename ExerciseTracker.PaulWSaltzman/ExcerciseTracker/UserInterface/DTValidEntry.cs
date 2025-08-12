using Spectre.Console;

namespace ExerciseTracker;

internal class DTValidEntry
{
    internal static DateTime GetDateTime()
    {
        var sessionDateTime = new DateTime();
        int year = AnsiConsole.Prompt(
        new TextPrompt<int>("Year YYYY:")
        .ValidationErrorMessage("[red] That is not a valid year.[/]")
        .Validate(year =>
        {
            if (year > DateTime.Now.Year)
            {
                return ValidationResult.Error("[red]You may not enter a future date[/]");
            }
            else if (year <= 2000)
            {
                return ValidationResult.Error("[red]You may not enter a date older than they year 2000.[/]");
            }
            else
            {
                return ValidationResult.Success();
            }
        }));

        int month = AnsiConsole.Prompt(
         new TextPrompt<int>("Month MM:")
         .ValidationErrorMessage("[red] That is not a valid month[/]")
         .Validate(month =>
         {

             if (month < 1 || month > 12)
             {
                 return ValidationResult.Error("[red]Valid months are 1-12 [/]");
             }
             else
             {

                 var sessionDateTime = new DateTime(year, month, 1, 0, 0, 0);
                 if (sessionDateTime > DateTime.Now)
                 {
                     return ValidationResult.Error("[red]You may not enter a future date[/]");
                 }
                 else
                 {
                     return ValidationResult.Success();
                 }
             }
         }));

        int day = AnsiConsole.Prompt(
        new TextPrompt<int>("Day DD:")
        .ValidationErrorMessage("[red] That is not a valid day[/]")
        .Validate(day =>
        {
            if (day < 1 || day > 31)
            {
                return ValidationResult.Error("[red]Valid days are 1-31 [/]");
            }
            else if ((sessionDateTime = new DateTime(year, month, day, 0, 0, 0)) > DateTime.Now)
            {
                return ValidationResult.Error("[red]You may not enter a future date[/]");
            }
            else
            {
                int daysInMonth = DateTime.DaysInMonth(year, month);

                if (day > daysInMonth)
                {
                    return ValidationResult.Error("[red]There are not that many days in the month.[/]");
                }
                else
                {
                    return ValidationResult.Success();
                }
            }
        }));

        int hour = AnsiConsole.Prompt(
         new TextPrompt<int>("24 Hour hh:")
         .ValidationErrorMessage("[red] That is not a valid hour[/]")
         .Validate(hour =>
         {
             if (0 > hour || hour > 23)
             {
                 return ValidationResult.Error("[red]Valid days are 0 - 23[/]");
             }
             else
             {
                 return ValidationResult.Success();
             }
         }));

        int min = AnsiConsole.Prompt(
         new TextPrompt<int>("Min mm:")
         .ValidationErrorMessage("[red] That is not a valid minute[/]")
         .Validate(min =>
         {
             if (0 > min || min > 59)
             {
                 return ValidationResult.Error("[red]Valid minutes are 0 - 59[/]");
             }
             else
             {
                 return ValidationResult.Success();
             }
         }));

        sessionDateTime = new DateTime(year, month, day, hour, min, 0);
        return sessionDateTime;
    }


    internal static DateTime GetEndDateTime(DateTime startDateTime)
    {
        int hour = AnsiConsole.Prompt(
            new TextPrompt<int>("24 Hour hh:")
            .ValidationErrorMessage("[red] That is not a valid hour[/]")
            .Validate(hour =>
            {
                if (0 > hour || hour > 23)
                {
                    return ValidationResult.Error("[red]Valid days are 0 - 23[/]");
                }
                else if (hour < startDateTime.Hour)
                {
                    return ValidationResult.Error("[red]Time may not be earlier than Start Time[/]");
                }
                else
                {
                    return ValidationResult.Success();
                }
            }));

        int min = AnsiConsole.Prompt(
         new TextPrompt<int>("Min mm:")
         .ValidationErrorMessage("[red] That is not a valid minute[/]")
         .Validate(min =>
         {
             if (0 > min || min > 59)
             {
                 return ValidationResult.Error("[red]Valid minutes are 0 - 59[/]");
             }
             else if (hour == startDateTime.Hour && min < startDateTime.Minute)
             {
                 return ValidationResult.Error("[red]Time may not be earlier than Start Time[/]");
             }
             else
             {
                 return ValidationResult.Success();
             }
         }));

        DateTime sessionDateTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, hour, min, 0);

        return sessionDateTime;
    }
}
