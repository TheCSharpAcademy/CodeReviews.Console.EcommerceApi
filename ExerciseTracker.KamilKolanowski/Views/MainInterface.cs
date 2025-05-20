using ExerciseTracker.KamilKolanowski.Controllers;
using ExerciseTracker.KamilKolanowski.Enums;
using ExerciseTracker.KamilKolanowski.Services;
using Spectre.Console;

namespace ExerciseTracker.KamilKolanowski.Views;

internal class MainInterface
{
    private readonly ExerciseController _controller;
    private readonly ExerciseService _service;

    public MainInterface(ExerciseController controller, ExerciseService service)
    {
        _controller = controller;
        _service = service;
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
                    AnsiConsole.MarkupLine("Press any key to continue...");
                    break;
                case ExerciseTrackerMenu.Menu.RemoveExercise:
                    _controller.DeleteExercise();
                    AnsiConsole.MarkupLine("Press any key to continue...");
                    break;
                case ExerciseTrackerMenu.Menu.ReadExercises:
                    _controller.ReadExercises();
                    AnsiConsole.MarkupLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case ExerciseTrackerMenu.Menu.Exit:
                    break;
            }
        }
    }

    
}
