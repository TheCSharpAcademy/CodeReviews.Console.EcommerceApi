using Spectre.Console;

internal class ExerciseMenu
{
    private static readonly Dictionary<string, Action> _menuActions = new()
    {
        { DisplayInfoHelpers.Back, Console.Clear },
        { "Show all exercises", ExerciseService.ShowAllExercises },
        { "Add new exercise", ExerciseService.CreateExercise },
        { "Edit exercise", ExerciseService.UpdateExercise },
        { "Delete exercise", ExerciseService.DeleteExercise }
    };

    internal static void ShowExerciseMenu()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an action: ")
            .PageSize(10)
            .AddChoices(_menuActions.Keys));

        _menuActions[choice]();
    }
}
