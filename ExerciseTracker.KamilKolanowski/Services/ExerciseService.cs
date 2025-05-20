using ExerciseTracker.KamilKolanowski.Models;
using ExerciseTracker.KamilKolanowski.Repositories;
using Spectre.Console;

namespace ExerciseTracker.KamilKolanowski.Services;

public class ExerciseService
{
    private readonly UserInputService _userInputService;
    private readonly IExerciseRepository _repository;
    private readonly ExerciseService _service;

    public ExerciseService(IExerciseRepository repository, UserInputService userInputService)
    {
        _repository = repository;
        _userInputService = userInputService;
    }
    
    internal void AddExercise()
    {
        var exercise = CreateExercise();

        _repository.Insert(exercise);
        Close();
    }

    internal void UpdateExercise()
    {
        var exerciseId = PromptForId("update");
        var exercise = _repository.GetExercise(exerciseId);
        
        if (exercise == null) return;

        var updatedExercise = _userInputService.EditExercise(exercise);

        _repository.Update(updatedExercise);
    }

    internal void DeleteExercise()
    {
        try
        {
            var exercises = GetExercisesTable();

            while (true)
            {
                var exerciseId = PromptForId("delete");

                if (!exercises.TryGetValue(exerciseId, out var mappedExercise))
                {
                    AnsiConsole.MarkupLine("[red]No exercise found with the provided id [/]");
                    continue;
                }
            
                var exercise = _repository.GetExercise(mappedExercise);

                if (exercise == null) return;

                _repository.Delete(exerciseId);
                AnsiConsole.MarkupLine("[green]Successfully deleted exercise![/]");
                Close();
            }
            
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
        }
    }

    internal void ReadExercises()
    {
        try
        {
            GetExercisesTable();
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
        }
    }

    private Exercise CreateExercise()
    {
        try
        {
            return _userInputService.CreateExercise();
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
            return null;
        }
        
    }
    
    private int PromptForId(string operation)
    {
        try
        {
            return AnsiConsole.Prompt(
                new TextPrompt<int>($"Select exercise id to {operation}:")
                    .Validate(id => _repository.GetExercise(id) != null));
            // fix here to validate after mapping the keys;
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
            return -1;
        }
    }
    private Dictionary<int, int> GetExercisesTable()
    {
        try
        {
            var table = new Table();

            table.AddColumn("[cyan]Exercise Id[/]");
            table.AddColumn("[cyan]Exercise Name[/]");
            table.AddColumn("[cyan]Start Datetime[/]");
            table.AddColumn("[cyan]End Datetime[/]");
            table.AddColumn("[cyan]Duration[/]");
            table.AddColumn("[cyan]Comment[/]");

            var exercises = _repository.GetExercises();
            var mappedId = new Dictionary<int, int>();
            var idx = 1;

            foreach (var exercise in exercises)
            {
                table.AddRow(
                    idx.ToString(),
                    exercise.Name,
                    exercise.DateStart.ToString("yyyy-MM-dd HH:mm:ss"),
                    exercise.DateEnd.ToString("yyyy-MM-dd HH:mm:ss"),
                    exercise.Duration.ToString(@"hh\:mm\:ss"),
                    exercise.Comment ?? ""
                );
                mappedId.Add(idx, exercise.Id);
                idx++;
            }

            table.Border = TableBorder.Rounded;
            AnsiConsole.Write(table);

            return mappedId;
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
            return null;
        }
        
    }

    private void Close()
    {
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
    }
}
