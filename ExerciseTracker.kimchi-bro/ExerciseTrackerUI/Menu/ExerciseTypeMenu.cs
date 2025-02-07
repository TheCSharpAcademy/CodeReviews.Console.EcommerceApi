using Spectre.Console;

internal class ExerciseTypeMenu
{
    private static readonly Dictionary<string, Action> _menuActions = new()
    {
        { DisplayInfoHelpers.Back, Console.Clear },
        { "Show all exercise types", ExerciseTypeService.ShowAllExerciseTypes },
        { "Add new exercise type", ExerciseTypeService.CreateExerciseType },
        { "Edit exercise type", ExerciseTypeService.UpdateExerciseType },
        { "Delete exercise type", ExerciseTypeService.DeleteExerciseType }
    };

    internal static void ShowExerciseTypeMenu()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an action: ")
            .PageSize(10)
            .AddChoices(_menuActions.Keys));

        _menuActions[choice]();
    }
}
