using Newtonsoft.Json;
using Spectre.Console;
using System.Text;
using ExerciseTrackerAPI.Models;

namespace ExerciseTrackerUI.MockArea;

internal class MockExerciseType
{
    internal static void Seed()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]Seeding exercise types[/]\n");

        var exerciseNames = new string[] { "Burpees", "Push-ups", "Squats", "Pull-ups", "Leg Raises" };
        var exerciseTypes = new List<ExerciseType>();

        foreach (var name in exerciseNames)
        {
            exerciseTypes.Add(new ExerciseType { Name = name });
        }

        AddExerciseType(exerciseTypes);
    }

    private static void AddExerciseType(List<ExerciseType> exerciseTypes)
    {
        try
        {
            var json = JsonConvert.SerializeObject(exerciseTypes);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = HttpClientFactory.GetHttpClient()
                .PostAsync(EndpointUrl.MockEndpoint + "/extadd", content).Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"[red]Failed to add exercise types into database.[/]\n" +
                    $"Status code: [yellow]{result.StatusCode}[/]");
            }

            AnsiConsole.MarkupLine("[green]New exercise types added successfully.[/]");
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

    internal static void DeleteAllExerciseTypes()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[yellow]Deleting all exercise types[/]\n");

        AnsiConsole.MarkupLine($"[red]That action will delete all exercise types and exercise records from database.[/]");
        if (!DisplayInfoHelpers.ConfirmDeletion())
        {
            Console.Clear();
            return;
        }

        DeleteExerciseTypes();
    }

    private static void DeleteExerciseTypes()
    {
        try
        {
            var result = HttpClientFactory.GetHttpClient()
                .DeleteAsync(EndpointUrl.MockEndpoint + "/extdel").Result;

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"[red]Failed to delete all exercise types and exercise records from database.[/]\n" +
                    $"Status code: [yellow]{result.StatusCode}[/]");
            }

            AnsiConsole.MarkupLine("[yellow]All exercise types and exercise records deleted successfully.[/]");
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
