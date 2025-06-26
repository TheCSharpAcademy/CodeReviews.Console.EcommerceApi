using ExerciseTracker.Controllers;
using ExerciseTracker.Models;
using Spectre.Console;

namespace ExerciseTracker.Menu;

public class Menu
{
    private readonly ExerciseController _controller;

    public Menu(ExerciseController controller)
    {
        _controller = controller;
    }

    public async Task AddExercise(Exercise exercise)
    {
        var choice = AnsiConsole.Prompt(new TextPrompt<string>("What would you like to do?")
            );
    }
}