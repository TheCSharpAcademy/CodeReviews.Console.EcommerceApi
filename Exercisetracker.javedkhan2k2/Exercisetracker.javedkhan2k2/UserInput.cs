
using System.Globalization;
using Exercisetacker.DTOs;
using Exercisetacker.Entities;
using Exercisetacker.UI;
using Exercisetacker.Validators;
using Shiftlogger.UI.Constants;
using Spectre.Console;

namespace Exercisetacker;

internal class UserInput
{
    internal static JoggingAddDto? GetNewJogging()
    {
        JoggingAddDto jogging = new JoggingAddDto();
        do
        {
            jogging.DateStart = GetDateInput("Enter Start date and time in [bold green](yyyy-MM-dd HH:mm:ss)[/] format.", "yyyy-MM-dd HH:mm:ss");
            if (jogging.DateStart == DateTime.MinValue) return null;

            jogging.DateEnd = GetDateInput("Enter End date and time in [bold green](yyyy-MM-dd HH:mm:ss)[/] format.", "yyyy-MM-dd HH:mm:ss");
            if (jogging.DateEnd == DateTime.MinValue) return null;

            if (ValidatorHelper.IsValidDateTimeInputs(jogging.ToJogging()))
            {
                jogging.Duration = jogging.DateEnd.Subtract(jogging.DateStart);
                break;
            }

        } while (true);
        jogging.Comments = GetStringInput("Enter Comments for the Jogging Session");
        if(jogging.Comments == "") return null;

        return jogging;
    }

    internal static bool GetUpdateJogging(Jogging jogging)
    {
        do
        {
            jogging.DateStart = AnsiConsole.Confirm($"{Messages.GetStartDateUpdate(jogging.DateStart)}") ? GetDateInput(Messages.StartTime, Messages.TimeFormat) : jogging.DateStart;
            if (jogging.DateStart == DateTime.MinValue) return false;

            jogging.DateEnd = AnsiConsole.Confirm($"{Messages.GetEndDateUpdate(jogging.DateEnd)}") ? GetDateInput(Messages.EndTime, Messages.TimeFormat) : jogging.DateEnd;
            if (jogging.DateEnd == DateTime.MinValue) return false;

            if (ValidatorHelper.IsValidDateTimeInputs(jogging))
            {
                jogging.Duration = jogging.DateEnd.Subtract(jogging.DateStart);
                break;
            }
            else
            {
                VisualizationEngine.DisplayDateTimeError();
            }

        } while (true);
        jogging.Comments = AnsiConsole.Confirm(Messages.GetCommentsUpdate(jogging.Comments)) ? GetStringInput(Messages.Comments) : jogging.Comments;
        if(jogging.Comments == "") return false;

        return true;
    }

    private static string GetStringInput(string message)
    {
        string? userInput = AnsiConsole.Ask<string?>($"{message} Or Enter 0 to cancel:");
        while(!ValidatorHelper.IsValidComment(userInput))
        {
            userInput = AnsiConsole.Ask<string?>($"[bold]Invalid input [red]({userInput})[/][/].\n{message} Or Enter 0 to cancel:");
        }
        return userInput == "0" ? "" : userInput;
    }

    internal static DateTime GetDateInput(string message, string format)
    {
        string? userInput = AnsiConsole.Ask<string?>($"{message} Or Enter 0 to cancel:");
        while (!ValidatorHelper.IsValidDateInput(userInput, format))
        {
            userInput = AnsiConsole.Ask<string?>($"[bold]Invalid input [red]({userInput})[/][/].\n{message} Or Enter 0 to cancel:");
        }
        return userInput == "0" ? DateTime.MinValue : DateTime.ParseExact(userInput, format, new CultureInfo("en-US"));
    }

    internal static int GetIntInput()
    {
        int id = AnsiConsole.Ask<int>("Enter an Id from the table Or Enter 0 to Cancel: ");
        return id;
    }

}