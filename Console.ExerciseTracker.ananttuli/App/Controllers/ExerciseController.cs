using System.Globalization;
using App.ExerciseLogs;
using App.ExerciseLogs.Models;
using App.Util;
using Spectre.Console;

namespace App.Controllers;

public class ExerciseController
{
    private readonly ExerciseService ExerciseLogsService;

    public ExerciseController(ExerciseService exerciseLogsService)
    {
        ExerciseLogsService = exerciseLogsService;
    }

    public async Task CreateLog()
    {
        await ListAllLogs();
        AnsiConsole.MarkupLine($"Create exercise log");
        var (startDateTime, endDateTime) = PromptDateTimes();
        var comments = PromptForComments();

        var created = await ExerciseLogsService.CreateExerciseLog(new ExerciseLogCreateDto(
            startDateTime,
            endDateTime,
            comments
        ));

        if (created == null)
            AnsiConsole.MarkupLine("[red]ERROR: Could not create exercise log[/]");
        else
            AnsiConsole.MarkupLine("[green]Created successfuly[/]");
    }

    public async Task ListAllLogs()
    {
        PrintLogs(await ExerciseLogsService.ListAllLogs());
    }

    public async Task UpdateLog()
    {
        await ListAllLogs();
        AnsiConsole.MarkupLine($"Edit exercise log");
        var exerciseLog = await PromptForExistingLog();
        var (startDateTime, endDateTime) = PromptDateTimes(exerciseLog.StartDateTime, exerciseLog.EndDateTime);
        var comments = PromptForComments(exerciseLog.Comments);

        exerciseLog.StartDateTime = startDateTime;
        exerciseLog.EndDateTime = endDateTime;
        exerciseLog.Comments = comments;

        var updated = await ExerciseLogsService.UpdateExerciseLog(exerciseLog.Id, exerciseLog);

        if (updated == null)
            AnsiConsole.MarkupLine("[red]ERROR: Could not update exercise log[/]");
        else
            AnsiConsole.MarkupLine("[green]Update successful[/]");
    }

    public async Task DeleteLog()
    {
        await ListAllLogs();
        AnsiConsole.MarkupLine($"Delete exercise log");

        var exerciseLog = await PromptForExistingLog();
        var deleteSuccessful = await ExerciseLogsService.DeleteExerciseLog(exerciseLog.Id);

        if (deleteSuccessful == true)
            AnsiConsole.MarkupLine("[green]Delete successful[/]");
        else
            AnsiConsole.MarkupLine("[red]ERROR: Could not update exercise log[/]");
    }

    private async Task<ExerciseLog> PromptForExistingLog()
    {
        while (true)
        {
            var exerciseLogId = AnsiConsole.Ask<int>("Enter exercise log ID: ");
            var log = await ExerciseLogsService.GetExerciseLog(exerciseLogId);
            if (log != null)
            {
                return log;
            }
            AnsiConsole.MarkupLine(
                "[red]Could not find a log with that ID." +
                " Please enter a valid ID from the list above[/]"
            );
        }
    }

    private string PromptForComments(string? existingComment = null)
    {
        return AnsiConsole.Ask("Comment: ", existingComment ?? "");
    }

    private static void PrintLogs(List<ExerciseLog> logs)
    {
        if (logs.Count == 0)
        {
            AnsiConsole.WriteLine("No logs found");
            return;
        }

        var table = new Table();

        table.AddColumns(["Id", "Start time", "End time", "Duration", "Comments"]);

        foreach (var log in logs)
        {
            table.AddRow([
                log.Id.ToString(), log.StartDateTime.ToString(), log.EndDateTime.ToString(),
                UiUtil.FormatDuration(log.Duration), log.Comments
            ]);
        }

        AnsiConsole.Write(table);
    }

    private Tuple<DateTime, DateTime> PromptDateTimes(DateTime? existingStartDateTime = null, DateTime? existingEndDateTime = null)
    {
        const string expectedDateTimeFormat = "yyyy-MM-dd HH:mm";

        AnsiConsole.MarkupLine("[grey]Note: Date times must be YYYY-mm-dd hh:mm with 24hr time e.g. [/][blue]2024-12-31 14:15[/]");

        string validStartDateTimeString = AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Start[/] date time: ")
                .PromptStyle("blue")
                .DefaultValue(existingStartDateTime?.ToString(expectedDateTimeFormat) ?? "")
                .ValidationErrorMessage("[red]That's not a valid date time[/]")
                .Validate(startDateTimeInput =>
                    {
                        bool validDate = DateTime.TryParseExact(
                           startDateTimeInput,
                           expectedDateTimeFormat,
                           CultureInfo.CurrentCulture,
                           DateTimeStyles.None,
                           out DateTime value
                       );

                        return validDate switch
                        {
                            false => ValidationResult.Error("\t[red]Please enter valid date format[/]"),
                            true => ValidationResult.Success()
                        };
                    }
                )
        );

        DateTime startDateTime = DateTime.Parse(validStartDateTimeString);

        string validEndDateTimeString = AnsiConsole.Prompt(
            new TextPrompt<string>("[green]End[/] date time: ")
                .PromptStyle("blue")
                .DefaultValue(existingEndDateTime?.ToString(expectedDateTimeFormat) ?? "")
                .ValidationErrorMessage("[red]That's not a valid date time[/]")
                .Validate(endDateTimeInput =>
                    {
                        bool validDate = DateTime.TryParseExact(
                           endDateTimeInput,
                           expectedDateTimeFormat,
                           CultureInfo.CurrentCulture,
                           DateTimeStyles.None,
                           out DateTime value
                       );

                        if (validDate == false)
                        {
                            return ValidationResult.Error("\t[red]Please enter valid date format[/]");
                        }

                        if (value < startDateTime)
                        {
                            return ValidationResult.Error("\t[red]End date time must be later than start date time[/]");
                        }

                        return ValidationResult.Success();
                    }
                )
        );

        DateTime endDateTime = DateTime.Parse(validEndDateTimeString);

        return new Tuple<DateTime, DateTime>(startDateTime, endDateTime);
    }
}