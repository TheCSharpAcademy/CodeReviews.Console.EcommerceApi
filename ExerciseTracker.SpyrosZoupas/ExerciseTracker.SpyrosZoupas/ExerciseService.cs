using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Net.Http.Json;
using System.Net.Http;
using ExerciseTracker.SpyrosZoupas.DAL;
using UserInterface.SpyrosZoupas.Util;

namespace ExerciseTracker.SpyrosZoupas;

public class ExerciseService
{
    private readonly ExerciseController _controller;

    public ExerciseService(ExerciseController controller)
    {
        _controller = controller;
    }

    public void InsertExercise()
    {
        DateTime startDate = InputValidation.GetDateTimeValue("Start Date:");
        DateTime endDate = InputValidation.GetEndDateTimeValue("End Date:", startDate);

        string comments = AnsiConsole.Ask<string>("Comments:");
        Exercise exercise = new Exercise { DateStart = startDate, DateEnd = endDate, Comments = comments };

        _controller.InsertExercise(exercise);
    }

    public Exercise? GetExercise()
    {
        Exercise? exercise = GetExerciseOptionInput();
        if (exercise == null)
        {
            return null;
        }
        else
        {
            return _controller.GetExerciseById(exercise.ExerciseId);
        }
    }

    public List<Exercise> GetAllExercises()
    {
        return _controller.GetAllExercises().ToList();
    }

    public void DeleteExercise()
    {
        Exercise? exercise = GetExerciseOptionInput();
        if (exercise == null) return;

        _controller.DeleteExercise(exercise);
    }

    public void UpdateExercise()
    {
        Exercise? exercise = GetExerciseOptionInput();
        if (exercise == null) return;

        if (AnsiConsole.Confirm("Update start date?"))
            exercise.DateStart = InputValidation.GetDateTimeValue("Updated start date:");
        if (AnsiConsole.Confirm("Update end date?"))
            exercise.DateEnd = InputValidation.GetEndDateTimeValue("End Date:", exercise.DateStart);
        if (AnsiConsole.Confirm("Update comments?"))
            exercise.Comments = AnsiConsole.Ask<string>("Updated comments:");

        _controller.UpdateExercise(exercise);
    }

    public Exercise? GetExerciseOptionInput()
    {
        var exercises = GetAllExercises();
        if (exercises.IsNullOrEmpty())
        {
            AnsiConsole.Markup($"[red]No Exercises found![/]");
            Console.ReadLine();
            Console.Clear();
            return null;
        }

        var option = AnsiConsole.Prompt(new SelectionPrompt<int>()
            .Title("Choose Exercise")
            .AddChoices(exercises.Select(s => s.ExerciseId)));

        return exercises.First(s => s.ExerciseId == option);
    }
}
