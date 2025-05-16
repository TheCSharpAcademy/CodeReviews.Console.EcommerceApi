using ExerciseTracker.SpyrosZoupas.DAL.Model;
using ExerciseTracker.SpyrosZoupas.Services;
using ExerciseTracker.SpyrosZoupas.Util;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using Table = Spectre.Console.Table;

namespace ExerciseTracker.SpyrosZoupas;

public class UserInput
{
    private readonly WeightExerciseService _exerciseWeightService;
    private readonly CardioExerciseService _exerciseCardioService;

    public UserInput(WeightExerciseService exerciseService, CardioExerciseService exerciseCardioService)
    {
        _exerciseWeightService = exerciseService;
        _exerciseCardioService = exerciseCardioService;
    }

    public void MainMenu()
    {
        var isMainMenuRunning = true;
        while (isMainMenuRunning)
        {
            Console.Clear();
            var option = AnsiConsole.Prompt(
            new SelectionPrompt<MainMenuOptions>()
            .Title("Main Menu")
            .AddChoices(
                MainMenuOptions.WeightExercise,
                MainMenuOptions.CardioExercise,
                MainMenuOptions.Quit));

            switch (option)
            {
                case MainMenuOptions.WeightExercise:
                    WeightExercisesMenu();
                    break;
                case MainMenuOptions.CardioExercise:
                    CardioExercisesMenu();
                    break;
                case MainMenuOptions.Quit:
                    isMainMenuRunning = false;
                    break;
            }
        }
    }

    public void WeightExercisesMenu()
    {
        var isWeightMenuRunning = true;
        while (isWeightMenuRunning)
        {
            Console.Clear();
            var option = AnsiConsole.Prompt(
            new SelectionPrompt<WeightExerciseMenuOptions>()
            .Title("WeightExercises Menu")
            .AddChoices(
                WeightExerciseMenuOptions.AddWeightExercise,
                WeightExerciseMenuOptions.DeleteWeightExercise,
                WeightExerciseMenuOptions.UpdateWeightExercise,
                WeightExerciseMenuOptions.ViewAllWeightExercises,
                WeightExerciseMenuOptions.ViewWeightExercise,
                WeightExerciseMenuOptions.Quit));

            switch (option)
            {
                case WeightExerciseMenuOptions.AddWeightExercise:
                    _exerciseWeightService.InsertWeightExercise();
                    break;
                case WeightExerciseMenuOptions.DeleteWeightExercise:
                    _exerciseWeightService.DeleteWeightExercise();
                    break;
                case WeightExerciseMenuOptions.UpdateWeightExercise:
                    _exerciseWeightService.UpdateWeightExercise();
                    break;
                case WeightExerciseMenuOptions.ViewWeightExercise:
                    ShowWeightExercise(_exerciseWeightService.GetWeightExercise());
                    break;
                case WeightExerciseMenuOptions.ViewAllWeightExercises:
                    ShowWeightExerciseTable(_exerciseWeightService.GetAllWeightExercises());
                    break;
                case WeightExerciseMenuOptions.Quit:
                    isWeightMenuRunning = false;
                    break;
            }
        }
    }

    public void CardioExercisesMenu()
    {
        var isCardioMenuRunning = true;
        while (isCardioMenuRunning)
        {
            Console.Clear();
            var option = AnsiConsole.Prompt(
            new SelectionPrompt<CardioExerciseMenuOptions>()
            .Title("CardioExercises Menu")
            .AddChoices(
                CardioExerciseMenuOptions.AddCardioExercise,
                CardioExerciseMenuOptions.DeleteCardioExercise,
                CardioExerciseMenuOptions.UpdateCardioExercise,
                CardioExerciseMenuOptions.ViewAllCardioExercises,
                CardioExerciseMenuOptions.ViewCardioExercise,
                CardioExerciseMenuOptions.Quit));

            switch (option)
            {
                case CardioExerciseMenuOptions.AddCardioExercise:
                    _exerciseCardioService.InsertCardioExercise();
                    break;
                case CardioExerciseMenuOptions.DeleteCardioExercise:
                    _exerciseCardioService.DeleteCardioExercise();
                    break;
                case CardioExerciseMenuOptions.UpdateCardioExercise:
                    _exerciseCardioService.UpdateCardioExercise();
                    break;
                case CardioExerciseMenuOptions.ViewCardioExercise:
                    ShowCardioExercise(_exerciseCardioService.GetCardioExercise());
                    break;
                case CardioExerciseMenuOptions.ViewAllCardioExercises:
                    ShowCardioExerciseTable(_exerciseCardioService.GetAllCardioExercises());
                    break;
                case CardioExerciseMenuOptions.Quit:
                    isCardioMenuRunning = false;
                    break;
            }
        }
    }

    private void ShowWeightExerciseTable(List<WeightExercise> exercises)
    {
        if (exercises.IsNullOrEmpty())
        {
            AnsiConsole.MarkupLine("[red]No data to display.[/]");
        }
        else
        {
            var table = new Table();
            table.AddColumn("Id")
                .AddColumn("Start")
                .AddColumn("End")
                .AddColumn("Duration")
                .AddColumn("Comments")
                .AddColumn("Mass");

            foreach (WeightExercise exercise in exercises)
            {
                table.AddRow(
                    exercise.Id.ToString(),
                    exercise.DateStart.ToString(),
                    exercise.DateEnd.ToString(),
                    $"{exercise.Duration.TotalHours:F2} hours",
                    exercise.Comments,
                    exercise.Mass.ToString());
            }

            AnsiConsole.Write(table);
        }

        Console.WriteLine("Enter any key to go back to Main Menu");
        Console.ReadLine();
        Console.Clear();
    }

    private void ShowCardioExerciseTable(List<CardioExercise> exercises)
    {
        if (exercises.IsNullOrEmpty())
        {
            AnsiConsole.MarkupLine("[red]No data to display.[/]");
        }
        else
        {
            var table = new Table();
            table.AddColumn("Id")
                .AddColumn("Start")
                .AddColumn("End")
                .AddColumn("Duration")
                .AddColumn("Comments")
                .AddColumn("Speed");

            foreach (CardioExercise exercise in exercises)
            {
                table.AddRow(
                    exercise.Id.ToString(),
                    exercise.DateStart.ToString(),
                    exercise.DateEnd.ToString(),
                    $"{exercise.Duration.TotalHours:F2} hours",
                    exercise.Comments,
                    exercise.Speed.ToString());
            }

            AnsiConsole.Write(table);
        }

        Console.WriteLine("Enter any key to go back to Main Menu");
        Console.ReadLine();
        Console.Clear();
    }

    private void ShowWeightExercise(WeightExercise? exercise)
    {
        if (exercise == null)
        {
            AnsiConsole.MarkupLine("[red]No data to display.[/]");
        }
        else
        {
            var panel = new Panel($@"Id: {exercise.Id}
Start: {exercise.DateStart}
End: {exercise.DateEnd}
Total hours: {exercise.Duration / 60 / 60}
Comments: {exercise.Comments}
Mass: {exercise.Mass} kg");
            panel.Header = new PanelHeader("Exercise Info");
            panel.Padding = new Padding(2, 2, 2, 2);

            AnsiConsole.Write(panel);
        }

        Console.WriteLine("Enter any key to go back to Main Menu");
        Console.ReadLine();
        Console.Clear();
    }

    private void ShowCardioExercise(CardioExercise? exercise)
    {
        if (exercise == null)
        {
            AnsiConsole.MarkupLine("[red]No data to display.[/]");
        }
        else
        {
            var panel = new Panel($@"Id: {exercise.Id}
Start: {exercise.DateStart}
End: {exercise.DateEnd}
Total hours: {exercise.Duration / 60 / 60}
Comments: {exercise.Comments}
Speed: {exercise.Speed} km/h");
            panel.Header = new PanelHeader("Exercise Info");
            panel.Padding = new Padding(2, 2, 2, 2);

            AnsiConsole.Write(panel);
        }

        Console.WriteLine("Enter any key to go back to Main Menu");
        Console.ReadLine();
        Console.Clear();
    }
}
