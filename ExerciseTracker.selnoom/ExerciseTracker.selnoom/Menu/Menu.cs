using ExerciseTracker.selnoom.Controllers;
using Spectre.Console;

namespace ExerciseTracker.selnoom.Menu;

public class Menu
{
    private readonly WeightExerciseController _weightExerciseController;

    public Menu(WeightExerciseController weightExerciseController)
    {
        _weightExerciseController = weightExerciseController;
    }

    public async Task ShowMenu()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup("[bold underline]Exercise Tracker[/]\n\n");
            AnsiConsole.WriteLine("Select an option:");
            var menuChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<MainMenuChoices>()
                        .Title("Please select an option:")
                        .AddChoices(Enum.GetValues<MainMenuChoices>())
            );

            switch (menuChoice)
            {
                case MainMenuChoices.View:
                    await ViewExercises();
                    break;
                case MainMenuChoices.Create:
                    //await CreateExercise();
                    break;
                case MainMenuChoices.Edit:
                    //await EditExercise();
                    break;
                case MainMenuChoices.Delete:
                    //await DeleteExercise();
                    break;
                case MainMenuChoices.Exit:
                    return;
            }
        }
    }

    private async Task ViewExercises()
    {
        AnsiConsole.Clear();
        try
        {
            var exercises = await _weightExerciseController.GetExercisesAsync();
            if (!exercises.Any())
            {
                AnsiConsole.MarkupLine("[red]No exercises registered.[/]");
                AnsiConsole.MarkupLine("\nPress enter to continue");
                AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
                return;
            }

            var sortedExercises = exercises.OrderBy(ex => ex.DateStart).ToList();

            AnsiConsole.MarkupLine("[blue bold underline]Exercises:[/]");
            foreach (var exercise in sortedExercises)
            {
                AnsiConsole.WriteLine($"Date Start: {exercise.DateStart}\tName: {exercise.Name}\tDuration: {exercise.Duration}\tSets: {exercise.Sets}\tRepetitions: {exercise.Repetitions}\tComments: {exercise.Comments}");
            }
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine("[red]There was an error while retrieving the exercises! Please try again later.[/]");
        }

        AnsiConsole.MarkupLine("\nPress enter to continue");
        AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
    }

    public enum MainMenuChoices
    {
        View,
        Create,
        Edit,
        Delete,
        Exit
    }
}
