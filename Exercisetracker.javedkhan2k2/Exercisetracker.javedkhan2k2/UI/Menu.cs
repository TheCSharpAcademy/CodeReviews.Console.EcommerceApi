using Spectre.Console;

namespace Exercisetacker.UI;

public class Menu
{
    public static string CancelOperation = $"[maroon]Go Back[/]";

    public string[] MainMenu = ["View All Sessions", "Add Jogging Session", "Update Jogging Session", "Delete Jogging Session", "Exit"];
    public string Title = "[yellow]Please Select An [blue]Action[/] From The Options Below[/]";
    internal string GetMainMenu()
    {
        return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(Title)
                    .PageSize(10)
                    .AddChoices(MainMenu)
        );
    }

}