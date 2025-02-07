using ExerciseTrackerAPI.Models;
using Newtonsoft.Json;
using Spectre.Console;
using System.Text;
using System.Text.Json;

internal class ExerciseService
{
    internal static void CreateExercise()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]Creating new exercise[/]\n");

        var (exit, exerciseType) = ExerciseTypeService.GetExerciseType();
        if (exit)
        {
            Console.Clear();
            return;
        }
        if (exerciseType.Id == 0)
        {
            AnsiConsole.MarkupLine("You need to add new exercise type first.\n");

            var answer = DisplayInfoHelpers.GetYesNoAnswer("Do you want to add new exercise type now?");
            if (!answer)
            {
                Console.Clear();
                return;
            }

            ExerciseTypeService.CreateExerciseType();
            var (_, newExerciseType) = ExerciseTypeService.GetExerciseType();
            exerciseType = newExerciseType;
        }

        var (exitTwo, startTime, endTime, duration, comments) = InputDataHelpers.GetData();
        if (exitTwo)
        {
            Console.Clear();
            return;
        }

        AddNewExercise(startTime, endTime, duration, comments, exerciseType);
    }

    private static void AddNewExercise(
        DateTime startTime, DateTime endTime, TimeSpan duration, string comments, ExerciseType exerciseType)
    {
        try
        {
            var exercise = new Exercise
            {
                ExerciseTypeName = exerciseType.Name,
                StartTime = startTime,
                EndTime = endTime,
                Duration = duration,
                Comments = comments
            };

            var json = JsonConvert.SerializeObject(exercise);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = HttpClientFactory.GetHttpClient()
                .PostAsync(EndpointUrl.ExercisesEndpoint, content).Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"[red]Failed to create new exercise record.[/]\n" +
                    $"Status code: [yellow]{result.StatusCode}[/]");
            }

            AnsiConsole.MarkupLine("[green]New exercise record created successfully.[/]");
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

    internal static void ShowAllExercises()
    {
        Console.Clear();
        var exercises = GetExerciseList();

        if (DisplayInfoHelpers.NoRecordsAvailable(exercises)) return;

        AnsiConsole.MarkupLine("[yellow]List of all exercises:[/]\n");

        int num = 1;
        var table = new Table();
        table.AddColumn("[yellow]No.[/]");
        table.AddColumn("[yellow]Exercise type[/]");
        table.AddColumn("[yellow]Start Time[/]");
        table.AddColumn("[yellow]End Time[/]");
        table.AddColumn("[yellow]Duration[/]");
        table.AddColumn("[yellow]Comments[/]");

        foreach (var exercise in exercises)
        {
            table.AddRow(
                new Markup($"[green]{num}[/]"),
                new Markup($"{exercise.ExerciseTypeName ?? string.Empty}"),
                new Markup($"{exercise.StartTime.ToString(InputDataHelpers.DateTimeFormat)}"),
                new Markup($"{exercise.EndTime.ToString(InputDataHelpers.DateTimeFormat)}"),
                new Markup($"{exercise.Duration.Hours:D2}:{exercise.Duration.Minutes:D2}"),
                new Markup($"{exercise.Comments ?? string.Empty}"));
            num++;
        }
        AnsiConsole.Write(table);
        AnsiConsole.Markup("\n[yellow]Press any key to continue...[/] ");
        Console.ReadKey(true);
        Console.Clear();
    }

    private static List<Exercise> GetExerciseList()
    {
        try
        {
            var result = HttpClientFactory.GetHttpClient()
                .GetAsync(EndpointUrl.ExercisesEndpoint).Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"[red]Failed to read exercise records from the database.[/]\n" +
                    $"Status code: [yellow]{result.StatusCode}[/]");
            }

            var json = result.Content.ReadAsStringAsync().Result;

            var jsonDocument = JsonDocument.Parse(json);

            var exercises = jsonDocument.RootElement.GetProperty("exercises").EnumerateArray()
                .Select(e => new Exercise
                {
                    Id = e.GetProperty("id").GetInt32(),
                    ExerciseTypeName = e.GetProperty("exerciseTypeName").GetString() ?? string.Empty,
                    StartTime = DateTime.Parse(e.GetProperty("startTime").GetString() ?? string.Empty),
                    EndTime = DateTime.Parse(e.GetProperty("endTime").GetString() ?? string.Empty),
                    Duration = TimeSpan.Parse(e.GetProperty("duration").GetString() ?? string.Empty),
                    Comments = e.GetProperty("comments").GetString() ?? string.Empty
                })
                .OrderBy(e => e.StartTime)
                .ToList();

            return exercises;
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

    private static Exercise GetExercise()
    {
        var exerciseMap = MakeExerciseMap();
        if (DisplayInfoHelpers.NoRecordsAvailable(exerciseMap.Keys)) return new Exercise();

        var choice = DisplayInfoHelpers.GetChoiceFromSelectionPrompt(
            "Choose exercise:", exerciseMap.Keys);
        if (choice == DisplayInfoHelpers.Back) return new Exercise();

        var success = exerciseMap.TryGetValue(choice, out Exercise? chosenExercise);
        if (!success) return new Exercise();

        return chosenExercise ?? new Exercise();
    }

    private static Dictionary<string, Exercise> MakeExerciseMap()
    {
        var exercises = GetExerciseList();
        var exerciseList = MakeExerciseList(exercises);
        var exerciseMap = new Dictionary<string, Exercise>();

        for (int i = 0; i < exercises.Count; i++)
        {
            exerciseMap.Add(exerciseList[i], exercises[i]);
        }
        return exerciseMap;
    }

    private static string ShowExercise(Exercise exercise)
    {
        return
            $"[yellow]{exercise.ExerciseTypeName}[/] " +
            $"[yellow]Start:[/] {exercise.StartTime.ToString(InputDataHelpers.DateTimeFormat)} " +
            $"[yellow]End:[/] {exercise.EndTime.ToString(InputDataHelpers.DateTimeFormat)} " +
            $"[yellow]Duration:[/] {exercise.Duration.Hours:D2}:{exercise.Duration.Minutes:D2} " +
            $"[green]Comments:[/] {exercise.Comments}";
    }

    private static List<string> MakeExerciseList(List<Exercise> exercises)
    {
        var tableData = new List<string>();
        int num = 1;
        foreach (var exercise in exercises)
        {
            tableData.Add(
                $"[green]{num}:[/] " +
                ShowExercise(exercise) +
                $"[{Console.BackgroundColor}] =>id:{exercise.Id}[/]");
            num++;
        }
        return tableData;
    }

    internal static void UpdateExercise()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]Updating exercise[/]\n");

        var exercise = GetExercise();
        if (exercise.Id == 0)
        {
            Console.Clear();
            return;
        }

        AnsiConsole.MarkupLine(ShowExercise(exercise));

        var (exit, exerciseType) = ExerciseTypeService.GetExerciseType();
        if (exit) 
        {
            Console.Clear();
            return;
        }

        var (exitTwo, startTime, endTime, duration, comments) = InputDataHelpers.GetData();
        if (exitTwo)
        {
            Console.Clear();
            return;
        }

        UpdateExerciseById(exercise.Id, exerciseType, startTime, endTime, duration, comments);
    }

    private static void UpdateExerciseById(
        int id, ExerciseType exerciseType, DateTime startTime, DateTime endTime, TimeSpan duration, string comments)
    {
        try
        {
            var exercise = new Exercise
            {
                Id = id,
                ExerciseTypeName = exerciseType.Name,
                StartTime = startTime,
                EndTime = endTime,
                Duration = duration,
                Comments = comments
            };

            var json = JsonConvert.SerializeObject(exercise);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = HttpClientFactory.GetHttpClient()
                .PutAsync(EndpointUrl.ExercisesEndpoint + $"/{id}", content).Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"[red]Failed to update exercise record.[/]\n" +
                    $"Status code: [yellow]{result.StatusCode}[/]");
            }

            AnsiConsole.MarkupLine("[green]Exercise record updated successfully.[/]");
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

    internal static void DeleteExercise()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]Deleting exercise[/]\n");

        var exercise = GetExercise();
        if (exercise.Id == 0)
        {
            Console.Clear();
            return;
        }

        AnsiConsole.MarkupLine($"[red]WARNING![/] You want to delete that exercise record permanently!");
        AnsiConsole.MarkupLine(ShowExercise(exercise));
        if (!DisplayInfoHelpers.ConfirmDeletion())
        {
            Console.Clear();
            return;
        }

        DeleteExerciseById(exercise.Id);
    }

    private static void DeleteExerciseById(int id)
    {
        try
        {
            var result = HttpClientFactory.GetHttpClient()
                .DeleteAsync(EndpointUrl.ExercisesEndpoint + $"/{id}").Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"[red]Failed to delete exercise record with Id: {id}.[/]\n" +
                    $"Status code: [yellow]{result.StatusCode}[/]");
            }

            AnsiConsole.MarkupLine("[yellow]Exercise record deleted successfully.[/]");
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
