using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using UserInterface.SpyrosZoupas.Util;
using ExerciseTracker.SpyrosZoupas.DAL.Model;
using ExerciseTracker.SpyrosZoupas.Controller;

namespace ExerciseTracker.SpyrosZoupas.Services;

public class WeightExerciseService
{
    private readonly WeightExerciseController _controller;

    public WeightExerciseService(WeightExerciseController controller)
    {
        _controller = controller;
    }

    public void InsertWeightExercise()
    {
        DateTime startDate = InputValidation.GetDateTimeValue("Start Date:");
        DateTime endDate = InputValidation.GetEndDateTimeValue("End Date:", startDate);

        string comments = AnsiConsole.Ask<string>("Comments:");
        double mass = AnsiConsole.Ask<double>("Mass:");
        WeightExercise WeightExercise = new WeightExercise { DateStart = startDate, DateEnd = endDate, Comments = comments, Mass = mass };

        _controller.InsertWeightExercise(WeightExercise);
    }

    public WeightExercise? GetWeightExercise()
    {
        WeightExercise? WeightExercise = GetWeightExerciseOptionInput();
        if (WeightExercise == null)
        {
            return null;
        }
        else
        {
            return _controller.GetWeightExerciseById(WeightExercise.Id);
        }
    }

    public List<WeightExercise> GetAllWeightExercises()
    {
        return _controller.GetAllWeightExercises().ToList();
    }

    public void DeleteWeightExercise()
    {
        WeightExercise? WeightExercise = GetWeightExerciseOptionInput();
        if (WeightExercise == null) return;

        _controller.DeleteWeightExercise(WeightExercise);
    }

    public void UpdateWeightExercise()
    {
        WeightExercise? WeightExercise = GetWeightExerciseOptionInput();
        if (WeightExercise == null) return;

        if (AnsiConsole.Confirm("Update start date?"))
            WeightExercise.DateStart = InputValidation.GetDateTimeValue("Updated start date:");
        if (AnsiConsole.Confirm("Update end date?"))
            WeightExercise.DateEnd = InputValidation.GetEndDateTimeValue("End Date:", WeightExercise.DateStart);
        if (AnsiConsole.Confirm("Update comments?"))
            WeightExercise.Comments = AnsiConsole.Ask<string>("Updated comments:");
        if (AnsiConsole.Confirm("Update mass?"))
            WeightExercise.Mass = AnsiConsole.Ask<double>("Updated mass:");

        _controller.UpdateWeightExercise(WeightExercise);
    }

    public WeightExercise? GetWeightExerciseOptionInput()
    {
        var exercises = GetAllWeightExercises();
        if (exercises.IsNullOrEmpty())
        {
            AnsiConsole.Markup($"[red]No Exercises found![/]");
            Console.ReadLine();
            Console.Clear();
            return null;
        }

        var option = AnsiConsole.Prompt(new SelectionPrompt<int>()
            .Title("Choose WeightExercise")
            .AddChoices(exercises.Select(s => s.Id)));

        return exercises.First(s => s.Id == option);
    }
}
