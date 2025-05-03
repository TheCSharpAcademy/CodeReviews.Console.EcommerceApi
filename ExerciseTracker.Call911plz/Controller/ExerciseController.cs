using System.Threading.Tasks;
using Spectre.Console;

public class ExerciseController(IService service) : ControllerBase
{
    IService _service = service;
    internal override void OnStartOfLoop()
    {
        Console.Clear();
        AnsiConsole.Write
        (
            new FigletText("Exercise Tracker")
                .LeftJustified()
                .Color(Color.Red)
        );
    }

    internal override async Task<bool> HandleUserInputAsync()
    {
        MenuEnums.Main input = GetMenu.MainMenu();

        switch (input)
        {
            case MenuEnums.Main.CREATE:
                await CreateAsync();
                break;
            case MenuEnums.Main.READ:
                await ReadByIdAsync();
                break;
            case MenuEnums.Main.READALL:
                ReadAll();
                break;
            case MenuEnums.Main.UPDATE:
                await UpdateAsync();
                break;
            case MenuEnums.Main.DELETE:
                await DeleteAsync();
                break;
            case MenuEnums.Main.EXIT:
                return true;
        }

        return false;
    }

    private async Task CreateAsync()
    {
        Exercise exercise = GetData.GetExercise();
        Exercise createdExercise = await _service.AddAsync(exercise);
        
        AnsiConsole.MarkupLine($"[bold grey]Inserted new[/] [bold yellow]Exercise[/] [bold grey]to db:[/]");
        DisplayData.DisplayExercise([createdExercise]);
    }

    private async Task ReadByIdAsync()
    {
        int id = GetData.GetId();
        Exercise exercise = await _service.GetByIdAsync(id)
        ?? throw new Exception("[bold red]Could not find exercise with id[/]");

        DisplayData.DisplayExercise([exercise]);
    }

    private void ReadAll()
    {
        List<Exercise> exercises = _service.GetAll() ?? [];

        DisplayData.DisplayExercise(exercises);
    }

    private async Task UpdateAsync()
    {
        List<Exercise> exercises = _service.GetAll() 
        ?? throw new Exception("No exercises to update");
        Exercise exerciseToUpdate = GetData.GetExerciseFromList(exercises);

        AnsiConsole.MarkupLine("[bold grey]Original: [/]");
        DisplayData.DisplayExercise([exerciseToUpdate]);

        Exercise updatedExercise = GetData.UpdateExercise(exerciseToUpdate);
        Exercise updatedExerciseInDb = await _service.UpdateAsync(updatedExercise);
        
        AnsiConsole.MarkupLine("[bold grey]Updated: [/]");
        DisplayData.DisplayExercise([updatedExerciseInDb]);
    }

    private async Task DeleteAsync()
    {
        List<Exercise> exercises = _service.GetAll()
        ?? throw new Exception("No exercises to delete");
        Exercise exerciseToDelete = GetData.GetExerciseFromList(exercises);
        await _service.DeleteAsync(exerciseToDelete);

        AnsiConsole.MarkupLine("[bold grey]Exercise[/] [bold red]deleted[/]");
    }
}