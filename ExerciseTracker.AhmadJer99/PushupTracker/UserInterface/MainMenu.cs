using Spectre.Console;

namespace ExerciseTracker.UserInterface;

public class MainMenu : BaseMenu
{
    private readonly ExerciseHistoryMenu _exerciseHistoryMenu;
    private readonly AddExerciseMenu _addExerciseMenu;

    private enum MenuOptions
    {
        AddExercise = 1,
        ExerciseHistory,
        Exit
    }

    public MainMenu( ExerciseHistoryMenu exerciseHistoryMenu, AddExerciseMenu addExerciseMenu)
    {
        _exerciseHistoryMenu = exerciseHistoryMenu;
        _addExerciseMenu = addExerciseMenu;
    }

    public override async Task ShowMenuAsync()
    {
        while (true)
        {
            Console.Clear();

            var selectedOption = AnsiConsole.Prompt(
            new SelectionPrompt<MenuOptions>()
            .Title("[teal]Main Menu[/]")
            .AddChoices(Enum.GetValues<MenuOptions>()));

            switch (selectedOption)
            {
                case MenuOptions.AddExercise:
                    await _addExerciseMenu.ShowMenuAsync();
                    break;
                case MenuOptions.ExerciseHistory:
                    await _exerciseHistoryMenu.ShowMenuAsync();
                    break;
                case MenuOptions.Exit:
                    AnsiConsole.MarkupLine("[red]Exiting...[/]");
                    return;
            }
        }
    }
}