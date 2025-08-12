using App.Util;
using Spectre.Console;

namespace App.Controllers;

public class AppController
{
    private readonly ExerciseController LogsController;

    public AppController(ExerciseController logsController)
    {
        LogsController = logsController;
    }

    public async Task Run()
    {
        var keepRunning = true;
        while (keepRunning) keepRunning = await ShowMenu();
    }

    public async Task<bool> ShowMenu()
    {
        string[] menuItems =
        [
            "Create log", "View logs",
            "Update log", "Delete log", "[red]Exit[/]"
        ];
        var option = PrintMainMenu(menuItems);

        try
        {
            switch (option)
            {
                case '1':
                    await LogsController.CreateLog();
                    break;
                case '2':
                    await LogsController.ListAllLogs();
                    break;
                case '3':
                    await LogsController.UpdateLog();
                    break;
                case '4':
                    await LogsController.DeleteLog();
                    break;
                case '5':
                    return false;
                default:
                    AnsiConsole.MarkupLine("[red]Please press one of the menu options above[/]");
                    break;
            }
        }
        catch (Exception ex)
        {
            if (ex is UserFacingException)
                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            else
                AnsiConsole.MarkupLine("[red]Error[/]");
        }

        UiUtil.PressKeyToContinue();

        return true;
    }

    private static char PrintMainMenu(string[] menuItems)
    {
        Console.Clear();
        AnsiConsole.MarkupLine("\n\nE X E R C I S E     T R A C K E R\n");
        AnsiConsole.Write(new Rows(
            menuItems.Select((menuItem, i) => new Markup($"{i + 1,-3} {menuItem,-3}"))
        ));

        AnsiConsole.Write("\n\nEnter option? ");

        var option = Console.ReadKey().KeyChar;

        AnsiConsole.WriteLine("\n");
        return option;
    }
}