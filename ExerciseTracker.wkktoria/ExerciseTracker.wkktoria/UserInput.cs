using ExerciseTracker.wkktoria.Controllers;
using ExerciseTracker.wkktoria.Data.Models;
using Spectre.Console;

namespace ExerciseTracker.wkktoria;

public class UserInput
{
    private readonly string _dateFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
    private readonly IExerciseController _exerciseController;

    private readonly string _timeFormat =
        Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern.Replace("tt", "").Trim();

    public UserInput(IExerciseController exerciseController)
    {
        _exerciseController = exerciseController;
    }

    public void Run()
    {
        var isAppRunning = true;

        while (isAppRunning)
        {
            Console.Clear();

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices("Add Exercise", "Edit Exercise", "Delete Exercise", "Show All Exercises",
                        "Show Exercise", "Quit")
            );

            switch (option)
            {
                case "Add Exercise":
                    AddExercise();
                    PressAnyKeyToContinue();
                    break;
                case "Edit Exercise":
                    EditExercise();
                    PressAnyKeyToContinue();
                    break;
                case "Delete Exercise":
                    DeleteExercise();
                    PressAnyKeyToContinue();
                    break;
                case "Show All Exercises":
                    ShowAllExercises();
                    PressAnyKeyToContinue();
                    break;
                case "Show Exercise":
                    ShowExercise();
                    PressAnyKeyToContinue();
                    break;
                case "Quit":
                    isAppRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private void ShowExercise()
    {
        try
        {
            var exercises = _exerciseController.GetAllExercises();

            Console.Clear();

            if (exercises.Any())
            {
                ShowExercisesTable(exercises);

                var id = AnsiConsole.Ask<int>("Enter id of exercise to show:");
                var exercise = _exerciseController.GetExercise(id);

                Console.Clear();

                while (exercise == null)
                {
                    WriteError($"No exercise with id '{id}'.");

                    ShowExercisesTable(exercises);

                    id = AnsiConsole.Ask<int>("Enter id of exercise to show:");
                    exercise = _exerciseController.GetExercise(id);
                }

                Console.Clear();

                ShowExerciseDetails(exercise);
            }
            else
            {
                Console.WriteLine("No exercises found.");
            }
        }
        catch (Exception e)
        {
            WriteError(e.Message);
        }
    }

    private static void ShowExerciseDetails(Exercise exercise)
    {
        var panel = new Panel($"""
                               Start Date: {exercise.StartDate:HH:mm, dd MMM yyyy}
                               End Date: {exercise.EndDate:HH:mm, dd MMM yyyy}
                               Duration: {exercise.Duration}
                               Comment: {exercise.Comment}
                               """)
        {
            Header = new PanelHeader($"Exercise#{exercise.Id}")
        };

        AnsiConsole.Write(panel);
    }

    private void ShowAllExercises()
    {
        try
        {
            var exercises = _exerciseController.GetAllExercises();

            Console.Clear();

            if (exercises.Any())
                ShowExercisesTable(exercises);
            else
                Console.WriteLine("No exercises found.");
        }
        catch (Exception e)
        {
            WriteError(e.Message);
        }
    }

    private static void ShowExercisesTable(List<Exercise> exercises)
    {
        var table = new Table();
        table.Title("Exercises");

        table.AddColumn("Id");
        table.AddColumn("Start Date");
        table.AddColumn("End Date");
        table.AddColumn("Duration");
        table.AddColumn("Comment");

        foreach (var exercise in exercises)
            table.AddRow(
                exercise.Id.ToString(),
                exercise.StartDate.ToString("HH:mm, dd MMM yyyy"),
                exercise.EndDate.ToString("HH:mm, dd MMM yyyy"),
                exercise.Duration.ToString(),
                exercise.Comment ?? string.Empty
            );

        AnsiConsole.Write(table);
    }

    private void DeleteExercise()
    {
        try
        {
            var exercises = _exerciseController.GetAllExercises();

            Console.Clear();

            if (exercises.Any())
            {
                ShowExercisesTable(exercises);

                var id = AnsiConsole.Ask<int>("Enter id of exercise to delete:");
                var exercise = _exerciseController.GetExercise(id);

                Console.Clear();

                while (exercise == null)
                {
                    WriteError($"No exercise with id '{id}'.");

                    ShowExercisesTable(exercises);

                    id = AnsiConsole.Ask<int>("Enter id of exercise to delete:");
                    exercise = _exerciseController.GetExercise(id);
                }

                _exerciseController.DeleteExercise(id);

                Console.Clear();
                WriteSuccess("Exercise has been deleted.");
            }
            else
            {
                Console.WriteLine("No exercises found.");
            }
        }
        catch (Exception e)
        {
            WriteError(e.Message);
        }
    }

    private void EditExercise()
    {
        try
        {
            var exercises = _exerciseController.GetAllExercises();

            Console.Clear();

            if (exercises.Any())
            {
                ShowExercisesTable(exercises);

                var id = AnsiConsole.Ask<int>("Enter id of exercise to edit:");
                var exercise = _exerciseController.GetExercise(id);

                Console.Clear();

                while (exercise == null)
                {
                    WriteError($"No exercise with id '{id}'.");

                    ShowExercisesTable(exercises);

                    id = AnsiConsole.Ask<int>("Enter id of exercise to edit:");
                    exercise = _exerciseController.GetExercise(id);
                }

                Console.Clear();

                DateTime fullStartDate;
                DateTime fullEndDate;
                TimeSpan duration;

                ShowExerciseDetails(exercise);

                do
                {
                    var startDate = AnsiConsole.Ask<DateTime>($"Enter start date (format: {_dateFormat}):");
                    var startTime = AnsiConsole.Ask<TimeSpan>($"Enter start time (format: {_timeFormat}):");
                    fullStartDate = startDate.Add(startTime);

                    var endDate = AnsiConsole.Ask<DateTime>($"Enter end date (format: {_dateFormat}):");
                    var endTime = AnsiConsole.Ask<TimeSpan>($"Enter end time (format: {_timeFormat}):");
                    fullEndDate = endDate.Add(endTime);

                    duration = fullEndDate - fullStartDate;

                    if (!Validation.IsValidDuration(duration)) WriteError("Duration cannot be longer than 24 hours.");

                    if (!Validation.IsStartDateBeforeEndDate(fullStartDate, fullEndDate))
                        WriteError("End date cannot be before start date.");
                } while (!Validation.IsStartDateBeforeEndDate(fullStartDate, fullEndDate) ||
                         !Validation.IsValidDuration(duration));

                var comment = AnsiConsole.Prompt(
                    new TextPrompt<string?>("Enter comment (or press enter to skip adding comment):")
                        .AllowEmpty());

                exercise.StartDate = fullStartDate;
                exercise.EndDate = fullEndDate;
                exercise.Duration = duration;
                exercise.Comment = comment;

                _exerciseController.UpdateExercise(exercise);

                Console.Clear();
                WriteSuccess("Exercise has been updated.");
            }
            else
            {
                Console.WriteLine("No exercises found.");
            }
        }
        catch (Exception e)
        {
            WriteError(e.Message);
        }
    }

    private void AddExercise()
    {
        DateTime fullStartDate;
        DateTime fullEndDate;
        TimeSpan duration;

        do
        {
            var startDate = AnsiConsole.Ask<DateTime>($"Enter start date (format: {_dateFormat}):");
            var startTime = AnsiConsole.Ask<TimeSpan>($"Enter start time (format: {_timeFormat}):");
            fullStartDate = startDate.Add(startTime);

            var endDate = AnsiConsole.Ask<DateTime>($"Enter end date (format: {_dateFormat}):");
            var endTime = AnsiConsole.Ask<TimeSpan>($"Enter end time (format: {_timeFormat}):");
            fullEndDate = endDate.Add(endTime);

            duration = fullEndDate - fullStartDate;

            if (!Validation.IsValidDuration(duration)) WriteError("Duration cannot be longer than 24 hours.");

            if (!Validation.IsStartDateBeforeEndDate(fullStartDate, fullEndDate))
                WriteError("End date cannot be before start date.");
        } while (!Validation.IsStartDateBeforeEndDate(fullStartDate, fullEndDate) ||
                 !Validation.IsValidDuration(duration));

        var comment = AnsiConsole.Prompt(
            new TextPrompt<string?>("Enter comment (or press enter to skip adding comment):")
                .AllowEmpty());

        var exercise = new Exercise
        {
            StartDate = fullStartDate,
            EndDate = fullEndDate,
            Duration = duration,
            Comment = comment
        };

        try
        {
            _exerciseController.AddExercise(exercise);

            Console.Clear();
            WriteSuccess("Exercise has been added.");
        }
        catch (Exception e)
        {
            WriteError(e.Message);
        }
    }

    private static void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    private static void WriteSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    private static void PressAnyKeyToContinue()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}