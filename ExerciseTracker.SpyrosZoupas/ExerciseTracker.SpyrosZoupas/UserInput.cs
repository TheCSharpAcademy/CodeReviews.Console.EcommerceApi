using ExerciseTracker.SpyrosZoupas.DAL;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace ExerciseTracker.SpyrosZoupas;

public class UserInput
{
    private readonly ExerciseService _exerciseService;

    public UserInput(ExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    public async Task ExercisesMenu()
    {
        var isContactMenuRunning = true;
        while (isContactMenuRunning)
        {
            Console.Clear();
            var option = AnsiConsole.Prompt(
            new SelectionPrompt<ExerciseMenuOptions>()
            .Title("Exercises Menu")
            .AddChoices(
                ExerciseMenuOptions.AddExercise,
                ExerciseMenuOptions.DeleteExercise,
                ExerciseMenuOptions.UpdateExercise,
                ExerciseMenuOptions.ViewAllExercises,
                ExerciseMenuOptions.ViewExercise,
                ExerciseMenuOptions.Quit));

            switch (option)
            {
                case ExerciseMenuOptions.AddExercise:
                    await _exerciseService.InsertExercise();
                    break;
                case ExerciseMenuOptions.DeleteExercise:
                    await _exerciseService.DeleteExercise();
                    break;
                case ExerciseMenuOptions.UpdateExercise:
                    await _exerciseService.UpdateExercise();
                    break;
                case ExerciseMenuOptions.ViewExercise:
                    ShowExercise(await _exerciseService.GetExercise());
                    break;
                case ExerciseMenuOptions.ViewAllExercises:
                    ShowExerciseTable(await _exerciseService.GetAllExercises());
                    break;
                case ExerciseMenuOptions.Quit:
                    isContactMenuRunning = false;
                    break;
            }
        }
    }

    private void ShowExerciseTable(List<Exercise> exercises)
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
                .AddColumn("Total hours");

            foreach (Exercise exercise in exercises)
            {
                table.AddRow(
                    exercise.ExerciseId.ToString(),
                    exercise.StartDateTime.ToString(),
                    exercise.EndDateTime.ToString(),
                    (exercise.DurationSeconds / 60 / 60).ToString());
            }

            AnsiConsole.Write(table);
        }

        Console.WriteLine("Enter any key to go back to Main Menu");
        Console.ReadLine();
        Console.Clear();
    }

    private void ShowExercise(Exercise? exercise)
    {
        if (exercise == null)
        {
            AnsiConsole.MarkupLine("[red]No data to display.[/]");
        }
        else
        {
            var panel = new Panel($@"Id: {exercise.ExerciseId}
Start: {exercise.StartDateTime}
End: {exercise.EndDateTime}
Total hours: {exercise.DurationSeconds / 60 / 60}");
            panel.Header = new PanelHeader("Exercise Info");
            panel.Padding = new Padding(2, 2, 2, 2);

            AnsiConsole.Write(panel);
        }

        Console.WriteLine("Enter any key to go back to Main Menu");
        Console.ReadLine();
        Console.Clear();
    }
}
