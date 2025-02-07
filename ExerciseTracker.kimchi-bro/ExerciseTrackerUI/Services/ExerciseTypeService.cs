using ExerciseTrackerAPI.Models;
using Newtonsoft.Json;
using Spectre.Console;
using System.Text;
using System.Text.Json;

internal class ExerciseTypeService
{
    internal static void CreateExerciseType()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]Adding new exercise type[/]\n");

        var name = GetExerciseTypeName();

        AddNewExerciseType(name);
    }

    private static void AddNewExerciseType(string name)
    {
        try
        {
            var exerciseType = new ExerciseType
            {
                Name = name,
            };

            var json = JsonConvert.SerializeObject(exerciseType);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = HttpClientFactory.GetHttpClient()
                .PostAsync(EndpointUrl.ExerciseTypesEndpoint, content).Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"[red]Failed to add new exercise type record.[/]\n" +
                    $"Status code: [yellow]{result.StatusCode}[/]");
            }

            AnsiConsole.MarkupLine("[green]New exercise type added successfully.[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
        }
        catch (HttpRequestException ex)
        {
            ErrorInfoHelpers.Http(ex);
        }
        catch (Exception ex)
        {
            ErrorInfoHelpers.General(ex);
        }
    }

    private static string GetExerciseTypeName()
    {
        var input = AnsiConsole.Ask<string>("Enter exercise type name:");
        while (input.Length < 3)
        {
            AnsiConsole.Markup("[red]Invalid input. Name must be at least 3 characters long.[/]\n");
            input = AnsiConsole.Ask<string>("Enter a valid exercise type name:");
        }
        return input;
    }

    internal static void ShowAllExerciseTypes()
    {
        Console.Clear();
        var exerciseTypes = GetExerciseTypesList();

        if (DisplayInfoHelpers.NoRecordsAvailable(exerciseTypes)) return;

        AnsiConsole.MarkupLine("[yellow]List of all exercise types:[/]\n");

        int num = 1;
        var table = new Table();
        table.AddColumn("[yellow]No.[/]");
        table.AddColumn("[yellow]Name[/]");

        foreach (var exerciseType in exerciseTypes)
        {
            table.AddRow(
                new Markup($"[green]{num}[/]"),
                new Markup($"{exerciseType.Name}"));
            num++;
        }
        AnsiConsole.Write(table);
        AnsiConsole.Markup("\n[yellow]Press any key to continue...[/] ");
        Console.ReadKey(true);
        Console.Clear();
    }

    internal static List<ExerciseType> GetExerciseTypesList()
    {
        try
        {
            var result = HttpClientFactory.GetHttpClient()
                .GetAsync(EndpointUrl.ExerciseTypesEndpoint).Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"[red]Failed to read exercise types list from the database.[/]\n" +
                    $"Status code: [yellow]{result.StatusCode}[/]");
            }

            var json = result.Content.ReadAsStringAsync().Result;

            var jsonDocument = JsonDocument.Parse(json);

            var exerciseTypes = jsonDocument.RootElement.GetProperty("exerciseTypes").EnumerateArray()
                .Select(e => new ExerciseType
                {
                    Id = e.GetProperty("id").GetInt32(),
                    Name = e.GetProperty("name").GetString() ?? string.Empty
                })
                .OrderBy(e => e.Name)
                .ToList();

            return exerciseTypes;
        }
        catch (HttpRequestException ex)
        {
            ErrorInfoHelpers.Http(ex);
            return [];
        }
        catch (Exception ex)
        {
            ErrorInfoHelpers.General(ex);
            return [];
        }
    }

    internal static (bool, ExerciseType) GetExerciseType()
    {
        var empty = new ExerciseType { Id = 0, Name = "" };

        var exerciseTypeMap = MakeExerciseTypeMap();

        if (!exerciseTypeMap.Any()) return (false, empty);

        var choice = DisplayInfoHelpers.GetChoiceFromSelectionPrompt(
            "Choose exercise type:", exerciseTypeMap.Keys);
        if (choice == DisplayInfoHelpers.Back) return (true, empty);

        var success = exerciseTypeMap.TryGetValue(choice, out ExerciseType? selectedExerciseType);
        if (!success) return (false, empty);

        if (selectedExerciseType is null) return (false, empty);
        else return (false, selectedExerciseType);
    }

    private static Dictionary<string, ExerciseType> MakeExerciseTypeMap()
    {
        var exerciseTypes = GetExerciseTypesList();
        var exerciseTypeList = MakeExerciseTypeList(exerciseTypes);
        var exerciseTypeMap = new Dictionary<string, ExerciseType>();

        for (int i = 0; i < exerciseTypes.Count; i++)
        {
            exerciseTypeMap.Add(exerciseTypeList[i], exerciseTypes[i]);
        }
        return exerciseTypeMap;
    }

    private static List<string> MakeExerciseTypeList(List<ExerciseType> exerciseTypes)
    {
        var tableData = new List<string>();
        int num = 1;
        foreach (var exerciseType in exerciseTypes)
        {
            tableData.Add(
                $"[green]{num}:[/] " +
                $"[yellow]{exerciseType.Name}[/]" +
                $"[{Console.BackgroundColor}] =>id:{exerciseType.Id}[/]");
            num++;
        }
        return tableData;
    }

    internal static void UpdateExerciseType()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]Editing exercise type's name[/]\n");

        var (exit, exerciseType) = GetExerciseType();
        if (exit)
        {
            Console.Clear();
            return;
        }
        if (exerciseType.Id == 0)
        {
            AnsiConsole.MarkupLine("[red]No records found in database.[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
            return;
        }

        AnsiConsole.MarkupLine($"[yellow]{exerciseType.Name}[/]\n");

        var newName = GetExerciseTypeName();

        UpdateExerciseTypeByName(exerciseType, newName);
    }

    private static void UpdateExerciseTypeByName(ExerciseType exerciseType, string newName)
    {
        try
        {
            exerciseType.Name = newName;

            var json = JsonConvert.SerializeObject(exerciseType);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = HttpClientFactory.GetHttpClient()
                .PutAsync(EndpointUrl.ExerciseTypesEndpoint + $"/{exerciseType.Id}", content).Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"[red]Failed to update exercise type's name.[/]\n" +
                    $"Status code: [yellow]{result.StatusCode}[/]");
            }

            AnsiConsole.MarkupLine("[green]Exercise type's name updated successfully.[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
        }
        catch (HttpRequestException ex)
        {
            ErrorInfoHelpers.Http(ex);
        }
        catch (Exception ex)
        {
            ErrorInfoHelpers.General(ex);
        }
    }

    internal static void DeleteExerciseType()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]Deleting exercise type[/]\n");

        var (exit, exerciseType) = GetExerciseType();
        if (exit)
        {
            Console.Clear();
            return;
        }
        if (exerciseType.Id == 0)
        {
            AnsiConsole.MarkupLine("[red]No records found in database.[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
            return;
        }

        AnsiConsole.MarkupLine($"[red]WARNING![/] You want to delete that exercise type permanently!");
        AnsiConsole.MarkupLine($"[yellow]{exerciseType.Name}[/]");
        if (!DisplayInfoHelpers.ConfirmDeletion())
        {
            Console.Clear();
            return;
        }

        DeleteExerciseTypeById(exerciseType.Id);
    }

    private static void DeleteExerciseTypeById(int id)
    {
        try
        {
            var result = HttpClientFactory.GetHttpClient()
                .DeleteAsync(EndpointUrl.ExerciseTypesEndpoint + $"/{id}").Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"[red]Failed to delete exercise type with Id: {id}.[/]\n" +
                    $"Status code: [yellow]{result.StatusCode}[/]");
            }

            AnsiConsole.MarkupLine("[yellow]Exercise type deleted successfully.[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
        }
        catch (HttpRequestException ex)
        {
            ErrorInfoHelpers.Http(ex);
        }
        catch (Exception ex)
        {
            ErrorInfoHelpers.General(ex);
        }
    }
}
