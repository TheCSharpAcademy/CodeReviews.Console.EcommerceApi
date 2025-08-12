using ExerciseTracker.frockett.Controllers;
using Spectre.Console;

namespace ExerciseTracker.frockett.UI;

public class MenuHandler
{
    private readonly ExerciseController controller;
    private readonly UserInput userInput;
    private readonly TableEngine tableEngine;

    public MenuHandler(ExerciseController controller, UserInput userInput, TableEngine tableEngine)
    {
        this.controller = controller;
        this.userInput = userInput;
        this.tableEngine = tableEngine;
    }   

    public void ShowMainMenu()
    {
        var menuSelection = new SelectionPrompt<MainMenuOptions>()
            .Title("Main Menu")
            .AddChoices(Enum.GetValues<MainMenuOptions>())
            .UseConverter(option => option.GetEnumDescription());

        var selection = AnsiConsole.Prompt(menuSelection);

        switch (selection)
        {
            case MainMenuOptions.ViewSessions:
                HandlesViewSessions();
                break;
            case MainMenuOptions.AddSession:
                HandleAddSession();
                break;
            case MainMenuOptions.UpdateSession:
                HandleUpdateSession();
                break;
            case MainMenuOptions.DeleteSession:
                HandleDeleteSession();
                break;
            case MainMenuOptions.Exit:
                Environment.Exit(0);
                break;
        }
    }

    private void HandlesViewSessions()
    {
        tableEngine.PrintSessions(controller.GetExerciseSessions());
        PauseForUser();
    }

    private void HandleAddSession()
    {
        bool response = controller.AddExerciseSession(userInput.GetNewSession());

        if (response)
        {
            AnsiConsole.WriteLine("Session added successfully!");
            PauseForUser();
        }
        else
        {
            AnsiConsole.WriteLine("An unknown error occured. Try again.");
            PauseForUser();
        }
    }

    private void HandleUpdateSession()
    {
        var sessionToUpdate = tableEngine.SelectSession(controller.GetExerciseSessions());
        if (sessionToUpdate == null)
        {
            PauseForUser();
            return;
        }

        var updatedSession = userInput.GetNewSession();
        updatedSession.Id = sessionToUpdate.Id;

        controller.UpdateExerciseSession(updatedSession);
        PauseForUser();
    }

    private void HandleDeleteSession()
    {
        var session = tableEngine.SelectSession(controller.GetExerciseSessions());
        if (session == null)
        {
            PauseForUser();
            return;
        }
        controller.RemoveExerciseSession(session);
        PauseForUser();
    }

    private void PauseForUser()
    {
        AnsiConsole.WriteLine("Press enter to continue...");
        Console.ReadLine();
        AnsiConsole.Clear();
        ShowMainMenu();
    }
}
