using Spectre.Console;
using Newtonsoft.Json;
using System.Text;
using ExerciseTrackerAPI.Models;

namespace ExerciseTrackerUI.MockArea;

internal class MockExercise
{
    internal static void Generate()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]Generating random exercise records.[/]\n");

        var exerciseTypes = ExerciseTypeService.GetExerciseTypesList();

        if (!exerciseTypes.Any())
        {
            AnsiConsole.MarkupLine("Add at least one exercise type first.");
            AnsiConsole.MarkupLine("You can do it manually from main menu or seed some exercise names.\n");

            var answer = DisplayInfoHelpers.GetYesNoAnswer("Do you want to seed new exercise types?");
            if (!answer)
            {
                Console.Clear();
                return;
            }

            MockExerciseType.Seed();
            exerciseTypes = ExerciseTypeService.GetExerciseTypesList();
        }

        var numberOfExercises = GetPositiveNumberInput("Enter a number of exercises to generate:");

        var exercises = GenerateExercises(numberOfExercises, exerciseTypes);

        AddExercises(exercises);
    }

    private static void AddExercises(List<Exercise> exercises)
    {
        try
        {
            var json = JsonConvert.SerializeObject(exercises);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = HttpClientFactory.GetHttpClient()
                .PostAsync(EndpointUrl.MockEndpoint + "/add", content).Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"[red]Failed to add exercise records into database.[/]\n" +
                    $"Status code: [yellow]{result.StatusCode}[/]");
            }

            AnsiConsole.MarkupLine("[green]New exercise records created successfully.[/]");
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

    private static List<Exercise> GenerateExercises(int number, List<ExerciseType> exerciseTypes)
    {
        var exercises = new List<Exercise>();
            for (int i = 0; i < number; i++)
        {
            var exerciseType = exerciseTypes[Random.Shared.Next(exerciseTypes.Count)].Name;
            var startTime = DateTime.Today.AddDays(Random.Shared.Next(-30, 1)) +
                TimeSpan.FromHours(Random.Shared.Next(7, 21));
            var duration = TimeSpan.FromMinutes(Random.Shared.Next(5, 56));
            var endTime = startTime + duration;
            var comments = "Test comment";

            exercises.Add(new Exercise
            {
                ExerciseTypeName = exerciseType,
                StartTime = startTime,
                EndTime = endTime,
                Duration = duration,
                Comments = comments
            });
        }
        return exercises.OrderBy(s => s.StartTime).ToList();
    }

    internal static int GetPositiveNumberInput(string message)
    {
        var input = AnsiConsole.Ask<int>(message);
        while (input <= 0)
        {
            AnsiConsole.Markup("[red]Invalid input. Only positive numbers accepted.[/]\n");
            input = AnsiConsole.Ask<int>("Enter a valid number:");
        }
        return input;
    }

    internal static void DeleteAllExercises()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]Deleting all exercise records[/]\n");

        AnsiConsole.MarkupLine($"[red]That action will delete all exercise records from database.[/]");
        if (!DisplayInfoHelpers.ConfirmDeletion())
        {
            Console.Clear();
            return;
        }

        DeleteExercises();
    }

    private static void DeleteExercises()
    {
        try
        {
            var result = HttpClientFactory.GetHttpClient()
                .DeleteAsync(EndpointUrl.MockEndpoint + "/del").Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"[red]Failed to delete all exercise records from database.[/]\n" +
                    $"Status code: [yellow]{result.StatusCode}[/]");
            }

            AnsiConsole.MarkupLine("[yellow]All exercise records deleted successfully.[/]");
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
