using ExerciseTracker.tonyissa.Models;
using ExerciseTracker.tonyissa.Services;
using Spectre.Console;

namespace ExerciseTracker.tonyissa.UI;

public class MainMenuController
{
    private readonly ExerciseService _service;

    public MainMenuController(ExerciseService service)
    {
        _service = service;
    }

    public void StartMainMenu()
    {
        while (true)
        {
            Console.Clear();
            var result = UserInput.GetMenuSelection(MenuOptions.MainMenu);

            switch (result)
            {
                case "Log a new exercise shift":
                    CreateNewSession();
                    break;
                case "View all exercise shifts":
                    ViewAll();
                    break;
                default:
                    return;
            }
        }
    }

    public void StartLogMenu(List<ExerciseSession> log)
    {
        var result = UserInput.GetMenuSelection(MenuOptions.LogMenu);

        switch (result)
        {
            case "Update":
                UpdateSession(log);
                break;
            case "Delete":
                DeleteSession(log);
                break;
            default:
                return;
        }
    }

    public void ViewAll()
    {
        var log = _service.GetExerciseLog();

        if (log.Count == 0)
        {
            AnsiConsole.WriteLine("No entries found. Press any key to continue...");
            Console.ReadKey();
            return;
        }

        var table = new Table { Title = new TableTitle("Log") };
        table.AddColumns("ID", "Start", "End", "Comments", "Duration (hours)");

        foreach (var item in log)
        {
            var id = item.Id.ToString();
            var start = item.Start.ToString("g");
            var end = item.End.ToString("g");
            var comments = item.Comments ?? "";
            var duration = item.Duration.ToString("g");

            table.AddRow(id, start, end, comments, duration);
        }

        AnsiConsole.Write(table);
        StartLogMenu(log);
    }

    public void CreateNewSession()
    {
        var session = UserInput.GetNewSession(null);
        _service.AddExerciseSession(session);
        AnsiConsole.WriteLine("\nSession added. Press any key to continue...");
        Console.ReadKey();
    }

    public void UpdateSession(List<ExerciseSession> log)
    {
        var oldSession = UserInput.GetSessionFromId(log);
        var newSession = UserInput.GetNewSession(oldSession);

        _service.UpdateSession(newSession);
    }

    public void DeleteSession(List<ExerciseSession> log)
    {
        var session = UserInput.GetSessionFromId(log);
        var confirmation = AnsiConsole.Confirm($"Are you sure you want to delete this session?");

        if (!confirmation) return;

        _service.RemoveSession(session);
    }
}