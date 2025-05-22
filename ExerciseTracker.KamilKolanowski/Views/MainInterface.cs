using ExerciseTracker.KamilKolanowski.Controllers;
using ExerciseTracker.KamilKolanowski.Enums;
using ExerciseTracker.KamilKolanowski.Services;
using Spectre.Console;

namespace ExerciseTracker.KamilKolanowski.Views;

internal class MainInterface
{
    private readonly ExerciseController _controller;

    public MainInterface(ExerciseController controller)
    {
        _controller = controller;
    }

    internal void Start()
    {
        while (true)
        {
            Console.Clear();
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select what would you like to do")
                    .AddChoices(ExerciseTrackerMenu.ExerciseTrackerMenuDictionary.Values)
            );

            var selectedChoice = ExerciseTrackerMenu
                .ExerciseTrackerMenuDictionary.FirstOrDefault(e => e.Value == selection)
                .Key;

            switch (selectedChoice)
            {
                case ExerciseTrackerMenu.Menu.AddExercise:
                    _controller.AddExercise();
                    break;
                case ExerciseTrackerMenu.Menu.EditExercise:
                    _controller.UpdateExercise();
                    break;
                case ExerciseTrackerMenu.Menu.RemoveExercise:
                    _controller.DeleteExercise();
                    break;
                case ExerciseTrackerMenu.Menu.ReadExercises:
                    _controller.ReadExercises();
                    GoBackToMainMenu();
                    break;
                case ExerciseTrackerMenu.Menu.Exit:
                    Environment.Exit(0);
                    break;
            }
        }
    }

    private void GoBackToMainMenu()
    {
        AnsiConsole.MarkupLine("Press any key to go back to the main menu...");
        Console.ReadKey();
    }
}
