using ExerciseTracker.selnoom.Controllers;
using ExerciseTracker.selnoom.Helpers;
using ExerciseTracker.selnoom.Models;
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
                    await CreateExercise();
                    break;
                case MainMenuChoices.Edit:
                    await UpdateExercise();
                    break;
                case MainMenuChoices.Delete:
                    await DeleteExercise();
                    break;
                case MainMenuChoices.Exit:
                    return;
            }
        }
    }

    private async Task ViewExercises()
    {
        AnsiConsole.Clear();

        await DisplayExercises();

        AnsiConsole.MarkupLine("\nPress enter to continue");
        AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
    }

    public async Task CreateExercise()
    {
        AnsiConsole.Clear();

        string employeeName = AnsiConsole.Prompt(new TextPrompt<string>("Enter the exercise name or 0 to return:"));
        if (employeeName == "0") return;

        double weight = Validation.ValidatePositiveDouble("Enter the weight used in the exercise or 0 to return:");
        if (weight == 0) return;

        int sets = Validation.ValidatePositiveInt("Enter the number of sets done or 0 to return:");
        if (sets == 0) return;

        int repetitions = Validation.ValidatePositiveInt("Enter the number of repetitions done in each set or 0 to return:");
        if (repetitions == 0) return;

        (string?, string?) times = GetStartAndEndTimes();
        if (times.Item1 == null || times.Item2 == null) return;

        WeightExercise newExercise = new WeightExercise
        {
            Name = employeeName,
            Weight = weight,
            Sets = sets,
            Repetitions = repetitions,
            DateStart = DateTime.Parse(times.Item1),
            DateEnd = DateTime.Parse(times.Item2)
        };

        try
        {
            var createdExercise = await _weightExerciseController.CreateExerciseAsync(newExercise);
            if (createdExercise == null)
            {
                AnsiConsole.MarkupLine("[red]Exercise creation failed.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[green]Exercise created successfully![/]");
            }
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine("[red]There was an error while creating the exercise! Please try again later.[/]");
        }

        AnsiConsole.MarkupLine("\nPress enter to continue");
        AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
    }

    public async Task UpdateExercise()
    {
        AnsiConsole.Clear();

        List<WeightExercise> exercises = await _weightExerciseController.GetExercisesAsync();

        if (!exercises.Any())
        {
            AnsiConsole.MarkupLine("[red]No exercises registered.[/]");
            AnsiConsole.MarkupLine("\nPress enter to continue");
            AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
            return;
        }

        var selectedExercise = ChooseExercise(exercises);
        if (selectedExercise == null) return;

        string employeeName = AnsiConsole.Prompt(new TextPrompt<string>("Enter the new exercise name or 0 to return:"));
        if (employeeName == "0") return;

        double weight = Validation.ValidatePositiveDouble("Enter the new weight used in the exercise or 0 to return:");
        if (weight == 0) return;

        int sets = Validation.ValidatePositiveInt("Enter the new number of sets done or 0 to return:");
        if (sets == 0) return;

        int repetitions = Validation.ValidatePositiveInt("Enter the new number of repetitions done in each set or 0 to return:");
        if (repetitions == 0) return;

        (string?, string?) times = GetStartAndEndTimes();
        if (times.Item1 == null || times.Item2 == null) return;

        WeightExercise newExercise = new WeightExercise
        {
            Id = selectedExercise.Id,
            Name = employeeName,
            Weight = weight,
            Sets = sets,
            Repetitions = repetitions,
            DateStart = DateTime.Parse(times.Item1),
            DateEnd = DateTime.Parse(times.Item2)
        };

        try
        {
            var updatedExercise = await _weightExerciseController.UpdateExerciseAsync(newExercise);
            if (updatedExercise == null)
            {
                AnsiConsole.MarkupLine("[red]Exercise update failed.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[green]Exercise updated successfully![/]");
            }
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine("[red]There was an error while updating the exercise! Please try again later.[/]");
        }

        AnsiConsole.MarkupLine("\nPress enter to continue");
        AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
    }

    public async Task DeleteExercise()
    {
        AnsiConsole.Clear();

        List<WeightExercise> exercises = await _weightExerciseController.GetExercisesAsync();
        if (!exercises.Any())
        {
            AnsiConsole.MarkupLine("[red]No exercises registered.[/]");
            AnsiConsole.MarkupLine("\nPress enter to continue");
            AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
            return;
        }

        var selectedExercise = ChooseExercise(exercises);
        if (selectedExercise == null) return;

        try
        {
            var deletedExercise = await _weightExerciseController.DeleteExerciseAsync(selectedExercise.Id);
            if (deletedExercise == null)
            {
                AnsiConsole.MarkupLine("[red]Exercise deletion failed.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[green]Exercise deleted successfully![/]");
            }
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine("[red]There was an error while deleting the exercise! Please try again later.[/]");
        }

        AnsiConsole.MarkupLine("\nPress enter to continue");
        AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
    }

    public static WeightExercise? ChooseExercise(List<WeightExercise> exercises)
    {
        try
        {
            var choices = new List<object> { "None" };
            choices.AddRange(exercises);

            object selected = AnsiConsole.Prompt(
                new SelectionPrompt<object>()
                    .Title("Select an exercise (choose 'None' to return to the menu):")
                    .PageSize(10)
                    .AddChoices(choices)
                    .UseConverter(obj =>
                    {
                        if (obj is string str)
                            return str;
                        else if (obj is WeightExercise exercise)
                            return $"Date Start: {exercise.DateStart}\tName: {exercise.Name}\tDuration: {exercise.Duration}\tSets: {exercise.Sets}\tRepetitions: {exercise.Repetitions}";
                        return string.Empty;
                    })
            );

            if (selected is string s && s == "None")
            {
                return null;
            }
            else if (selected is WeightExercise exercise)
            {
                return exercise;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]There was an error when retrieving the exercises! Please try again later.[/]");
            AnsiConsole.MarkupLine("\nPress enter to continue");
            AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
            return null;
        }
    }

    public async Task DisplayExercises()
    {
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
                AnsiConsole.WriteLine($"Date Start: {exercise.DateStart}\tName: {exercise.Name}\tDuration: {exercise.Duration}\tSets: {exercise.Sets}\tRepetitions: {exercise.Repetitions}");
            }
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine("[red]There was an error while retrieving the exercises! Please try again later.[/]");
        }
    }

    internal static (string?, string?) GetStartAndEndTimes()
    {
        string? startTime;
        string? endTime;

        startTime = Validation.ValidateTimeInput();
        if (startTime == null)
        {
            return (null, null);
        }

        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold]Now, type the ending time of your exercise or 0 to return[/]");
        endTime = Validation.ValidateEndTimeInput(startTime);
        if (endTime == null)
        {
            return (null, null);
        }

        return (startTime, endTime);
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
