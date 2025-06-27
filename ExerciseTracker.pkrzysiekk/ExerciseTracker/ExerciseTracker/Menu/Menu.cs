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
                    await _controller.AddExercise(ex);
                    AnsiConsole.MarkupLine($"[Green]Exercise added[/]");
                    break;
                case MenuChoicesEnum.DeleteExercise:
                    var listToShow =await  _controller.GetAll();
                    if (!listToShow.Any())
                    {
                        AnsiConsole.MarkupLine($"[Red]Nothing to delete[/]");
                        continue;
                    }
                    TableHelper.Show(listToShow);
                    var IDToDelete = AnsiConsole.Prompt(new TextPrompt<int>("Type an ID to delete"));
                    var exerciseToDelete= listToShow.FirstOrDefault(x => x.Id==IDToDelete);
                    if (exerciseToDelete == null)
                    {
                        AnsiConsole.MarkupLine($"[Red]Cannot delete exercise[/]");
                        continue;
                    }
                    await _controller.DeleteExercise(exerciseToDelete);
                    AnsiConsole.MarkupLine($"[Green]Exercise deleted[/]");
                    
                    break;
                case MenuChoicesEnum.EditExercise:
                    var listToUpdate = await  _controller.GetAll();
                    if (!listToUpdate.Any())
                    {
                        AnsiConsole.MarkupLine($"[Red]Nothing to update[/]");
                        continue;
                    }
                    TableHelper.Show(listToUpdate);
                    var id= AnsiConsole.Prompt(new TextPrompt<int>("Type an ID to update"));
                    var exerciseToUpdate= listToUpdate.FirstOrDefault(x => x.Id==id);
                    if (exerciseToUpdate == null)
                    {
                        AnsiConsole.MarkupLine($"[Red]Cannot update exercise[/]");
                        continue;
                    }
                    var exerciseToEdit=UserInput.GetExercise();
                    exerciseToEdit.Id = id;
                    await _controller.UpdateExercise(exerciseToEdit);
                    AnsiConsole.MarkupLine($"[Green]Exercise updated[/]");
                    break;
                case MenuChoicesEnum.GetAllExercises:
                    var list= await _controller.GetAll();
                    if (!list.Any())
                    {
                        AnsiConsole.MarkupLine($"[Red]Nothing to list[/]");
                        continue;
                    }
                    TableHelper.Show(list);
                    
                    break;
            }    
        }
        
        
    }
}