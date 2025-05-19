using ExerciseTracker.KamilKolanowski.Controllers;
using ExerciseTracker.KamilKolanowski.Enums;
using ExerciseTracker.KamilKolanowski.Models;
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
                    break;
                case ExerciseTrackerMenu.Menu.Exit:
                    break;
            }
        }
    }

    internal void ShowTable()
    {
        var table = new Table();

        table.AddColumn("[cyan]Exercise Id[/]");
        table.AddColumn("[cyan]Start Datetime[/]");
        table.AddColumn("[cyan]End Datetime[/]");
        table.AddColumn("[cyan]Duration[/]");
        table.AddColumn("[cyan]Comment[/]");

        var exercises = _service.ReadExercises();
        var idx = 1;
        
        foreach (var exercise in exercises)
        {
            table.AddRow(
                idx.ToString(),
                exercise.DateStart.ToString("yyyy-MM-dd HH:mm:ss"),
                exercise.DateEnd.ToString("yyyy-MM-dd HH:mm:ss"),
                exercise.Duration.ToString(@"hh\:mm\:ss"),
                exercise.Comment ?? ""
            );
        }
        
        table.Border = TableBorder.Rounded;
        AnsiConsole.Write(table);
    }
}
