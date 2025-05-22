using ExerciseTracker.KamilKolanowski.Enums;
using ExerciseTracker.KamilKolanowski.Interfaces;
using ExerciseTracker.KamilKolanowski.Models;
using Spectre.Console;

namespace ExerciseTracker.KamilKolanowski.Services;

public class ExerciseService
{
    private readonly UserInputService _userInputService;
    private readonly IExerciseRepositoryFactory _repositoryFactory;

    public ExerciseService(IExerciseRepositoryFactory repositoryFactory, UserInputService userInputService)
    {
        _repositoryFactory = repositoryFactory;
        _userInputService = userInputService;
    }
    
    internal void AddExercise()
    {
        try
        {
            var exerciseType = _userInputService.GetUserChoiceForExerciseType();
            var repo = _repositoryFactory.GetExerciseRepository(exerciseType);
            var exercise = CreateExercise(exerciseType.ToString());

            repo.Insert(exercise);
            
            Close("added");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
        }
    }

    internal void UpdateExercise()
    {
        try
        {
            var exerciseId = PromptForId("update");
            
            var efRepo = _repositoryFactory.GetExerciseRepository(ExerciseType.Weight);
            var dapperRepo = _repositoryFactory.GetExerciseRepository(ExerciseType.Cardio);

            var exercise = efRepo.GetExercise(exerciseId) ?? dapperRepo.GetExercise(exerciseId);

            if (exercise == null)
                return;

            var repo = _repositoryFactory.GetExerciseRepository(Enum.Parse<ExerciseType>(exercise.ExerciseType));

            var updatedExercise = _userInputService.EditExercise(exercise);
            repo.Update(updatedExercise);

            Close("edited");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
        }
    }

    internal void DeleteExercise()
    {
        try
        {
            var exerciseId = PromptForId("delete");

            var efRepo = _repositoryFactory.GetExerciseRepository(ExerciseType.Weight);
            var dapperRepo = _repositoryFactory.GetExerciseRepository(ExerciseType.Cardio);

            var exercise = efRepo.GetExercise(exerciseId) ?? dapperRepo.GetExercise(exerciseId);

            if (exercise == null)
                return;

            var repo = _repositoryFactory.GetExerciseRepository(Enum.Parse<ExerciseType>(exercise.ExerciseType));
            repo.Delete(exerciseId);

            Close("deleted");
            Console.ReadKey();
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

    private Exercise CreateExercise(string exerciseType)
    {
        try
        {
            return _userInputService.CreateExercise(exerciseType);
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
                var exerciseId = AnsiConsole.Ask<int>(
                    $"Choose [yellow2]exercise id[/] to {operation}:"
                );

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
            var exerciseType = _userInputService.GetUserChoiceForExerciseType();
            var repo = _repositoryFactory.GetExerciseRepository(exerciseType);
            
            var table = new Table();

            table.AddColumn("[cyan]Exercise Id[/]");
            table.AddColumn("[cyan]Exercise Name[/]");
            table.AddColumn("[cyan]Start Datetime[/]");
            table.AddColumn("[cyan]End Datetime[/]");
            table.AddColumn("[cyan]Duration[/]");
            table.AddColumn("[cyan]Comment[/]");
            table.AddColumn("[cyan]Exercise Type[/]");

            var exercises = repo.GetExercises(exerciseType.ToString());
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
                    exercise.Comment ?? "",
                    exercise.ExerciseType
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

    private void Close(string operation)
    {
        if (operation is not null)
        {
            AnsiConsole.MarkupLine($"[green]Successfully {operation} exercise![/]");
        }
        AnsiConsole.MarkupLine("Press any key to continue...");
    }
}
