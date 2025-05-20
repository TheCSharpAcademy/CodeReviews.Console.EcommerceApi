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
        Close("added");
        Console.ReadKey();
    }

    internal void UpdateExercise()
    {
        var exerciseId = PromptForId("update");
        var exercise = _repository.GetExercise(exerciseId);

        if (exercise == null)
            return;

        var updatedExercise = _userInputService.EditExercise(exercise);

        _repository.Update(updatedExercise);
        Close("edited");
        Console.ReadKey();
    }

    internal void DeleteExercise()
    {
        try
        {
            var exerciseId = PromptForId("delete");
            
            _repository.Delete(exerciseId);

            Close("deleted");
        }
        
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
        }

        Console.ReadKey();
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
            var exercises = GetExercisesTable();
            
            while (true)
            {
                var exerciseId = AnsiConsole.Ask<int>($"Choose [yellow2]exercise id[/] to {operation}:");
                
                if (!exercises.TryGetValue(exerciseId, out var mappedExerciseId))
                {
                    AnsiConsole.MarkupLine("[red]No exercise found with the provided id [/]");
                    continue;
                }
                
                return mappedExerciseId;
            }
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
                mappedId.Add(exercise.Id, idx);
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

    private void Close(string operation)
    {
        if (operation is not null)
        {
            AnsiConsole.MarkupLine($"[green]Successfully {operation} exercise![/]");
        }
        AnsiConsole.MarkupLine("Press any key to continue...");
    }
}
