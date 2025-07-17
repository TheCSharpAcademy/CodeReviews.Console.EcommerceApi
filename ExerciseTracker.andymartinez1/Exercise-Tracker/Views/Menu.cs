using Exercise_Tracker.Controllers;
using Exercise_Tracker.Enums;
using Exercise_Tracker.Utils;
using Spectre.Console;

namespace Exercise_Tracker.Views;

public class Menu
{
    private readonly ExerciseController _exerciseController;

    public Menu(ExerciseController exerciseController)
    {
        _exerciseController = exerciseController;
    }

    private readonly MenuOptions[] _options =
    [
        MenuOptions.ViewAllExercises,
        MenuOptions.ViewExerciseById,
        MenuOptions.AddNewExercise,
        MenuOptions.UpdateExercise,
        MenuOptions.DeleteExercise,
        MenuOptions.Exit,
    ];

    public void MainMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
        {
            AnsiConsole.Write(new FigletText("Exercise Tracker"));

            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                    .Title("Select an option:")
                    .PageSize(10)
                    .AddChoices(_options)
                    .UseConverter(c => c.GetDisplayName())
            );

            switch (userChoice)
            {
                case MenuOptions.ViewAllExercises:
                    AnsiConsole.Clear();
                    _exerciseController.GetAllExercises();
                    break;
                case MenuOptions.ViewExerciseById:
                    AnsiConsole.Clear();
                    _exerciseController.GetExerciseById();
                    break;
                case MenuOptions.AddNewExercise:
                    AnsiConsole.Clear();
                    _exerciseController.AddExercise();
                    break;
                case MenuOptions.UpdateExercise:
                    AnsiConsole.Clear();
                    _exerciseController.UpdateExercise();
                    break;
                case MenuOptions.DeleteExercise:
                    AnsiConsole.Clear();
                    _exerciseController.DeleteExercise();
                    break;
                case MenuOptions.Exit:
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine(
                        "[blue]Thank you for using this Exercise Tracker! Press any key to exit.[/]"
                    );
                    Console.ReadKey();
                    isMenuRunning = false;
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
