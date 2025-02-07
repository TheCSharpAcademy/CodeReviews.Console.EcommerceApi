using ExerciseTrackerUI.MockArea;
using Spectre.Console;

internal class MockMenu
{
    private static readonly Dictionary<string, Action> _menuActions = new()
    {
        { DisplayInfoHelpers.Back, Console.Clear },
        { "[green]Generate random exercises[/]", MockExercise.Generate },
        { "[green]Seed exercise types[/]", MockExerciseType.Seed },
        { "[yellow]Delete all exercises[/]", MockExercise.DeleteAllExercises },
        { "[red]Delete all exercise types[/]", MockExerciseType.DeleteAllExerciseTypes }
    };

    internal static void ShowMockMenu()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an action: ")
            .PageSize(10)
            .AddChoices(_menuActions.Keys));

        _menuActions[choice]();
    }
}
