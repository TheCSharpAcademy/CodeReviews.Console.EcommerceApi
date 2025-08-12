using ExerciseTracker.Service;
using static ExerciseTracker.Models.Enums;
using Spectre.Console;
using ExerciseTracker.Input;
using ExerciseTracker.Models;

namespace ExerciseTracker.Controller;

public class ExerciseController(IService service, IInput input) : IController
{
    private readonly IService _service = service;
    private readonly IInput _input = input;

    public async Task MainMenu()
    {
        while (true)
        {
            AnsiConsole.Clear();
            switch (_input.GetMenuOption())
            {
                case MainMenuOption.ShowAllExercises:
                    await ShowAllExercisesAsync();
                    AnsiConsole.MarkupLine("Press enter to return to main menu...");
                    Console.ReadLine();
                    break;
                case MainMenuOption.Add:
                    await AddExerciseAsync();
                    break;
                case MainMenuOption.Update:
                    await UpdateExerciseAsync();
                    break;
                case MainMenuOption.Remove:
                    await DeleteExerciseByIdAsync();
                    break;
                case MainMenuOption.Quit:
                    Environment.Exit(0);
                    return;
            }
        }
    }
    public async Task<List<Exercise>?> ShowAllExercisesAsync()
    {
        List<Exercise> exercises = await _service.GetAllExercisesAsync();

        if (exercises == null || exercises.Count == 0)
        {
            AnsiConsole.Markup("[red]No exercises[/] ");
            return null;
        }

        Table table = new() { Title = new TableTitle("All Exercises") };
        table.AddColumn("Id");
        table.AddColumn("Start");
        table.AddColumn("End");
        table.AddColumn("Duration");
        table.AddColumn("Comments");
        table.AddColumn("Type");

        foreach (Exercise exercise in exercises)
        {
            table.AddRow(exercise.Id.ToString(), exercise.DateStart.ToString(), exercise.DateEnd.ToString(), exercise.Duration.ToString(), exercise.Comments, exercise.Type.ToString());
        }

        AnsiConsole.Write(table);
        return exercises;
    }

    public async Task AddExerciseAsync()
    {
        (DateTime start, DateTime end) = _input.GetDates();
        await _service.AddExerciseAsync(start, end, _input.GetComments(), _input.GetType());
    }

    public async Task UpdateExerciseAsync()
    {
        IEnumerable<Exercise>? exercises = await ShowAllExercisesAsync();

        if (exercises == null || !exercises.Any())
        {
            AnsiConsole.MarkupLine("Press enter to return to menu...");
            Console.ReadLine();
            return;
        }

        try
        {
            int id = _input.GetId("Update");
            (DateTime start, DateTime end) = _input.GetDates();
            string comments = _input.GetComments();
            ExerciseType type = _input.GetType();
            await _service.UpdateExerciseAsync(id, start, end, comments, type);
        }
        catch (ApplicationException ex)
        {
            AnsiConsole.MarkupLine($"[red]Unable to update:[/] {ex.Message}");
            Console.ReadLine();
        }
    }

    public async Task DeleteExerciseByIdAsync()
    {
        List<Exercise>? exercises = await ShowAllExercisesAsync();

        if (exercises == null || exercises.Count == 0)
        {
            AnsiConsole.MarkupLine("Press enter to return to menu...");
            Console.ReadLine();
            return;
        }

        try
        {
            await _service.DeleteExerciseByIdAsync(_input.GetId("Delete"));
        }
        catch (ApplicationException ex)
        {
            AnsiConsole.MarkupLine($"[red]Unable to delete:[/] {ex.Message}");
            Console.ReadLine();
        }
    }
}