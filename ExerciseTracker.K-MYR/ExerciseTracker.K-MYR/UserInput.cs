using Spectre.Console;
using System.Globalization;
using System.Text;
using static ExerciseTracker.K_MYR.Enums;

namespace ExerciseTracker.K_MYR;

internal class UserInput
{
    private readonly IExerciseController _ExerciseController;
    public UserInput(IExerciseController exerciseController)
    {
        _ExerciseController = exerciseController;
    }

    public async Task ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.Write(new Panel("[springgreen2_1]Exercise Tracker[/]").BorderColor(Color.DarkOrange3_1));
            var choice = AnsiConsole.Prompt(new SelectionPrompt<MainMenu>()
                                                .AddChoices(Enum.GetValues(typeof(MainMenu)).Cast<MainMenu>()));

            switch (choice)
            {
                case MainMenu.AddExercise:
                    await AddExercise();
                    break;
                case MainMenu.UpdateExercise:
                    await UpdateExercise();
                    break;
                case MainMenu.DeleteExercise:
                    await DeleteExercise();
                    break;
                case MainMenu.ViewExercise:
                    ShowExercise();
                    break;
                case MainMenu.ViewAllExercises:
                    ShowAllExercises();
                    break;
                case MainMenu.Quit:
                    Console.Clear();
                    AnsiConsole.Write(new Panel("[springgreen2_1]GOODBYE[/]").BorderColor(Color.DarkOrange3_1));
                    Environment.Exit(0);
                    break;
            }
        }
    }

    private async Task AddExercise()
    {
        Console.Clear();

        (var startTime, var endTime) = GetExerciseTimes();
        var exerciseType = GetExerciseType();

        Console.Write("Comments: ");
        var comments = Console.ReadLine() ?? "";

        try
        {
            await _ExerciseController.AddAsync(new ExerciseInsertModel
            {
                Type = exerciseType,
                StartTime = startTime,
                EndTime = endTime,
                Comments = comments
            });
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Panel($"An Error Occured Saving The Entity: {ex.Message}"));
        }
    }

    private async Task UpdateExercise()
    {
        Console.Clear();

        var training = GetExercise();

        if (training is not null)
        {
            if (AnsiConsole.Confirm("Update Exercise Times?", false))
            {
                (training.StartTime, training.EndTime) = GetExerciseTimes();
                training.Duration = (training.EndTime - training.StartTime).Ticks;
            }

            if (AnsiConsole.Confirm("Update Exercise Type?", false))
                training.Type = GetExerciseType();

            if (AnsiConsole.Confirm("Update Comments?", false))
            {
                Console.Write("Comments: ");
                training.Comments = Console.ReadLine() ?? "";
            }

            try
            {
                await _ExerciseController.UpdateAsync(training);

            }
            catch (Exception ex)
            {
                AnsiConsole.Write(new Panel($"An Error Occured Updating The Entity: {ex.Message}"));
            }
        }
    }

    private async Task DeleteExercise()
    {
        Console.Clear();

        var training = GetExercise();

        if (training is not null)
        {
            try
            {
                await _ExerciseController.DeleteAsync(training);
            }
            catch (Exception ex)
            {
                AnsiConsole.Write(new Panel($"An Error Occured Deleting The Entity: {ex.Message}"));
            }
        }
    }

    private void ShowAllExercises()
    {
        try
        {
            Console.Clear();
            PrintAllExercises(_ExerciseController.GetAll().OrderBy(e => e.StartTime).ToArray());
            Helpers.PrintAndWait("Press Any Key To Return");
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Panel($"An Error Occured Getting The Entities: {ex.Message}"));
        }
    }

    private void ShowExercise()
    {
        try
        {
            var exercise = GetExercise();

            if (exercise is null)
                return;

            var table = new Table()
                        .BorderColor(Color.DarkOrange3_1)
                        .AddColumns("[springgreen2_1]Type[/]", "[springgreen2_1]Start Time[/]", "[springgreen2_1]End Time[/]",
                         "[springgreen2_1]Duration[/]");

            var duration = TimeSpan.FromTicks(exercise.Duration);

            table.AddRow(exercise.Type,
                        exercise.StartTime.ToString("dd/MM/yyyy HH:mm"),
                        exercise.EndTime.ToString("dd/MM/yyyy HH:mm"),
                        string.Format("{0} h {1} m", duration.Hours + duration.Days * 24, duration.Minutes));

            var commentsPanel = new Panel(exercise.Comments)
                .Header("[springgreen2_1]Comments[/]")
                .BorderColor(Color.DarkOrange3_1);

            Console.Clear();
            AnsiConsole.Write(new Panel(new Rows(table, commentsPanel))
                                    .BorderColor(Color.DarkOrange3_1));
            Helpers.PrintAndWait("Press Any Key To Return");
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Panel($"An Error Occured Getting The Entity: {ex.Message}"));
        }
    }

    private string GetExerciseType()
    {
        string type;

        var input = AnsiConsole.Prompt(new SelectionPrompt<ExerciseTypes>()
                                            .Title("Choose A Exercise Type: ")
                                            .AddChoices(Enum.GetValues(typeof(ExerciseTypes)).Cast<ExerciseTypes>()));

        if (input == ExerciseTypes.Custom)
            type = AnsiConsole.Ask<string>("Please Enter The Exercise Type:");
        else
            type = input.ToString();

        return type;
    }

    private DateTime GetDate(string text, string format = "dd-mm-yy hh:mm", string preset = "")
    {
        var sb = new StringBuilder();
        bool enterPressed;
        ConsoleKeyInfo key;
        DateTime date;

        do
        {
            enterPressed = false;

            Console.Write($"{text}: ");
            Console.Write(format);
            Console.CursorLeft -= format.Length;
            Console.Write(preset);

            sb.Clear();
            sb.Append(preset);

            while (!enterPressed)
            {
                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Backspace:
                        if (sb.Length > 0)
                        {
                            sb.Remove(sb.Length - 1, 1);
                            Console.CursorLeft -= 1;
                            Console.Write(format[sb.Length]);
                            Console.CursorLeft -= 1;
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (sb.Length == format.Length)
                        {
                            enterPressed = true;
                            Console.Write('\n');
                        }
                        break;
                    default:
                        if (sb.Length < format.Length && !char.IsControl(key.KeyChar))
                        {
                            sb.Append(key.KeyChar);
                            AnsiConsole.Write($"{key.KeyChar}");
                        }
                        break;
                }
            }
        } while (!DateTime.TryParseExact(sb.ToString().Trim(), "dd-MM-yy HH:mm", new CultureInfo("de-DE"), DateTimeStyles.None, out date));

        return date;
    }

    private (DateTime, DateTime) GetExerciseTimes()
    {
        var startTime = GetDate("Training Start Time");
        var endTime = GetDate("Training End Time", preset: startTime.ToString("dd-MM-yy "));

        while (endTime <= startTime)
        {
            AnsiConsole.Write(new Panel("[red]The Shift End Time Cannot Be Or Be Before The Shift Start Time[/]").BorderColor(Color.DarkOrange3_1));
            startTime = GetDate("Shift Start Time");
            endTime = GetDate("Shift End Time", preset: startTime.ToString("dd-MM-yy "));
        }

        return (startTime, endTime);
    }

    private Exercise? GetExercise()
    {
        var trainings = _ExerciseController.GetAll().OrderBy(e => e.StartTime).ToArray();

        PrintAllExercises(trainings);

        var id = AnsiConsole.Ask<int>("Please enter the Id or 0 to return: ");

        while (id < 0 || id > trainings.Length)
        {
            AnsiConsole.Write(new Markup("[red]Invalid Input[/]\n"));
            id = AnsiConsole.Ask<int>("Please enter the Id or 0 to return: ");
        }

        if (id == 0)
            return null;

        return trainings[id - 1];
    }

    private void PrintAllExercises(Exercise[] data)
    {
        var table = new Table()
                        .BorderColor(Color.DarkOrange3_1)
                        .AddColumns("[springgreen2_1]ID[/]", "[springgreen2_1]Type[/]", "[springgreen2_1]Start Time[/]", "[springgreen2_1]End Time[/]",
                         "[springgreen2_1]Duration[/]", "[springgreen2_1]Comments[/]");

        var exercises = data.AsSpan();

        TimeSpan duration;

        for (int i = 0; i < exercises.Length; i++)
        {
            duration = TimeSpan.FromTicks(exercises[i].Duration);
            table.AddRow((i + 1).ToString(),
                        exercises[i].Type,
                        exercises[i].StartTime.ToString("dd/MM/yyyy HH:mm"),
                        exercises[i].EndTime.ToString("dd/MM/yyyy HH:mm"),
                        string.Format("{0} h {1} m", duration.Hours + duration.Days * 24, duration.Minutes),
                        exercises[i].Comments.Length > 15 ? exercises[i].Comments[..12] + "..." : exercises[i].Comments);
        }

        AnsiConsole.Write(table);
    }
}
