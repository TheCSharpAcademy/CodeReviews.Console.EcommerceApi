using ExerciseTracker.Controllers;
using ExerciseTracker.Menu.MenuHelpers;
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

    private MenuChoicesEnum GetMenuChoice()
    {
        
        var choice = AnsiConsole.Prompt(new SelectionPrompt<MenuChoicesEnum>()
            .Title("What would you like to do?")
            .AddChoices(Enum.GetValues<MenuChoicesEnum>())
            .UseConverter(x => x.GetDescription()));
        return choice;
    }
    
    public async Task ShowMenu()
    {
        var choice = GetMenuChoice();

        while (choice != MenuChoicesEnum.Exit)
        {
            
            choice = GetMenuChoice();
            switch (choice)
            {
                case MenuChoicesEnum.AddExercise:
                    var ex = UserInput.GetExercise();
                    Console.WriteLine(ex);
                    break;
                case MenuChoicesEnum.DeleteExercise:
                    //
                    break;
                case MenuChoicesEnum.EditExercise:
                    //
                    break;
                case MenuChoicesEnum.GetAllExercises:
                    //
                    break;
            }    
        }
        
        
    }
}