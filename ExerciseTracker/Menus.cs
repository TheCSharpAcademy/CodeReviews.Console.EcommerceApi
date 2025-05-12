using ExerciseTracker.Controllers;
using ExerciseTracker.Models;
using ExerciseTracker.Repositories;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using static ExerciseTracker.Enums;

namespace ExerciseTracker;
class Menus
{
    private readonly DbContext _context;

    internal Menus(DbContext dbContext)
    {
        _context = dbContext;
    }

    internal void MainMenu()
    {
        bool run = true;
        while (run)
        {
            AnsiConsole.Clear();
            MainMenuOptions option = AnsiConsole.Prompt(new SelectionPrompt<MainMenuOptions>().Title("What do you want to do?")
                .AddChoices(MainMenuOptions.ManageFieldTours, MainMenuOptions.ManageFreeKicks, MainMenuOptions.Exit));
            switch (option)
            {
                case MainMenuOptions.ManageFieldTours:
                    ExerciceRepository<FieldTours> tours = new(_context);
                    FieldTourMenu(tours);
                    break;
                case MainMenuOptions.ManageFreeKicks:
                    FreeKicksMenu();
                    break;
                default:
                    run = false;
                    AnsiConsole.WriteLine("Thanks for using Exercise Tracker.");
                    break;
            }
        }
    }

    private void FreeKicksMenu()
    {
        AnsiConsole.Clear();
        ExerciceService exerciceService = new(_context, "");
        ExerciceMenuOptions option = AnsiConsole.Prompt(new SelectionPrompt<ExerciceMenuOptions>().Title("What do you want to do next?")
            .AddChoices(ExerciceMenuOptions.ShowAllExercices, ExerciceMenuOptions.ShowSpecificExercice, ExerciceMenuOptions.AddExercice, ExerciceMenuOptions.DeleteExercice, ExerciceMenuOptions.UpdateExercice, ExerciceMenuOptions.Return));
        switch (option)
        {
            case ExerciceMenuOptions.ShowAllExercices:
                exerciceService.PrintExercices();
                break;
            case ExerciceMenuOptions.ShowSpecificExercice:
                exerciceService.PrintOneExercice();
                break;
            case ExerciceMenuOptions.AddExercice:
                exerciceService.CreateExercice();
                break;
            case ExerciceMenuOptions.DeleteExercice:
                exerciceService.DeleteExercice();
                break;
            case ExerciceMenuOptions.UpdateExercice:
                exerciceService.UpdateExercice();
                break;
            default: break;
        }
        AnsiConsole.WriteLine("Press any key to continue.");
        Console.ReadLine();
    }

    private void FieldTourMenu(ExerciceRepository<FieldTours> tours)
    {
        AnsiConsole.Clear();
        ExerciceService exerciceService = new(_context, "FieldTours");
        ExerciceMenuOptions option = AnsiConsole.Prompt(new SelectionPrompt<ExerciceMenuOptions>().Title("What do you want to do next?")
            .AddChoices(ExerciceMenuOptions.ShowAllExercices, ExerciceMenuOptions.ShowSpecificExercice, ExerciceMenuOptions.AddExercice, ExerciceMenuOptions.DeleteExercice, ExerciceMenuOptions.UpdateExercice, ExerciceMenuOptions.Return));
        Console.Clear();
        switch (option)
        {
            case ExerciceMenuOptions.ShowAllExercices:
                exerciceService.PrintExercices();
                break;
            case ExerciceMenuOptions.ShowSpecificExercice:
                exerciceService.PrintOneExercice();
                break;
            case ExerciceMenuOptions.AddExercice:
                exerciceService.CreateExercice();
                break;
            case ExerciceMenuOptions.DeleteExercice:
                exerciceService.DeleteExercice();
                break;
            case ExerciceMenuOptions.UpdateExercice:
                exerciceService.UpdateExercice();
                break;
            default: break;
        }
        AnsiConsole.WriteLine("Press any key to continue.");
        Console.ReadLine();
    }
}