using Spectre.Console;

internal class MainMenu
{
    private static readonly Dictionary<string, Action> _menuActions = new()
    {
        { "Exercises", ExerciseMenu.ShowExerciseMenu },
        { "Exercise type", ExerciseTypeMenu.ShowExerciseTypeMenu },
        { "[yellow]Mock Area[/]", MockMenu.ShowMockMenu },
        { "Exit the app", () =>
            {
                Console.Clear();
                AnsiConsole.MarkupLine("[yellow]Goodbye![/]");
                Environment.Exit(0);
            }
        }
    };

    internal static void ShowMainMenu()
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose an action: ")
                .PageSize(10)
                .AddChoices(_menuActions.Keys));

            _menuActions[choice]();
        }
    }
}
