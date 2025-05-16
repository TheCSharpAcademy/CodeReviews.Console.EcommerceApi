using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using UserInterface.SpyrosZoupas.Util;
using ExerciseTracker.SpyrosZoupas.DAL.Model;
using ExerciseTracker.SpyrosZoupas.Controller;

namespace ExerciseTracker.SpyrosZoupas.Services;

public class CardioExerciseService
{
    private readonly CardioExerciseController _controller;

    public CardioExerciseService(CardioExerciseController controller)
    {
        _controller = controller;
    }

    public void InsertCardioExercise()
    {
        DateTime startDate = InputValidation.GetDateTimeValue("Start Date:");
        DateTime endDate = InputValidation.GetEndDateTimeValue("End Date:", startDate);

        string comments = AnsiConsole.Ask<string>("Comments:");
        double speed = AnsiConsole.Ask<double>("Speed:");
        CardioExercise CardioExercise = new CardioExercise { DateStart = startDate, DateEnd = endDate, Comments = comments, Speed = speed };

        _controller.InsertCardioExercise(CardioExercise);
    }

    public CardioExercise? GetCardioExercise()
    {
        CardioExercise? CardioExercise = GetCardioExerciseOptionInput();
        if (CardioExercise == null)
        {
            return null;
        }
        else
        {
            return _controller.GetCardioExerciseById(CardioExercise.Id);
        }
    }

    public List<CardioExercise> GetAllCardioExercises()
    {
        return _controller.GetAllCardioExercises().ToList();
    }

    public void DeleteCardioExercise()
    {
        CardioExercise? CardioExercise = GetCardioExerciseOptionInput();
        if (CardioExercise == null) return;

        _controller.DeleteCardioExercise(CardioExercise);
    }

    public void UpdateCardioExercise()
    {
        CardioExercise? CardioExercise = GetCardioExerciseOptionInput();
        if (CardioExercise == null) return;

        if (AnsiConsole.Confirm("Update start date?"))
            CardioExercise.DateStart = InputValidation.GetDateTimeValue("Updated start date:");
        if (AnsiConsole.Confirm("Update end date?"))
            CardioExercise.DateEnd = InputValidation.GetEndDateTimeValue("End Date:", CardioExercise.DateStart);
        if (AnsiConsole.Confirm("Update comments?"))
            CardioExercise.Comments = AnsiConsole.Ask<string>("Updated comments:");
        if (AnsiConsole.Confirm("Update speed?"))
            CardioExercise.Speed = AnsiConsole.Ask<double>("Updated speed:");

        _controller.UpdateCardioExercise(CardioExercise);
    }

    public CardioExercise? GetCardioExerciseOptionInput()
    {
        var exercises = GetAllCardioExercises();
        if (exercises.IsNullOrEmpty())
        {
            AnsiConsole.Markup($"[red]No Exercises found![/]");
            Console.ReadLine();
            Console.Clear();
            return null;
        }

        var option = AnsiConsole.Prompt(new SelectionPrompt<int>()
            .Title("Choose CardioExercise")
            .AddChoices(exercises.Select(s => s.Id)));

        return exercises.First(s => s.Id == option);
    }
}
