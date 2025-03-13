using ExerciseTracker.Core.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace ExerciseTracker.UI;

internal static class Menu
{
    private static readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://localhost:7223/")
    };

    internal static async Task AddExercise()
    {
        Console.Write("Enter Session Start Date (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
        {
            Console.WriteLine("Invalid date format. Use 'yyyy-MM-dd HH:mm'.");
            return;
        }

        Console.Write("Enter Session End Date (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime endDate) || endDate <= startDate)
        {
            Console.WriteLine("Invalid date format or end date is before the start date.");
            return;
        }

        Console.Write("Enter Comments: ");
        var comments = Console.ReadLine()?.Trim() ?? string.Empty;

        var exercise = new ExerciseDTO
        {
            StartDate = startDate,
            EndDate = endDate,
            Comments = comments
        };

        var response = await _httpClient.PostAsJsonAsync("api/exercises", exercise);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var createdExercise = JsonSerializer.Deserialize<ExerciseDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (createdExercise != null)
            {
                Console.WriteLine("Exercise added successfully.");
            }
            else
            {
                Console.WriteLine("Failed to parse the server response.");
            }
        }
        else
        {
            Console.WriteLine($"An error occurred: {response.ReasonPhrase}");
        }
    }

    internal static async Task UpdateExercise()
    {
        Console.Write("Enter Exercise ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int exerciseId))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var checkResponse = await _httpClient.GetAsync($"api/exercises/{exerciseId}");
        if (!checkResponse.IsSuccessStatusCode)
        {
            Console.WriteLine("Exercise not found.");
            return;
        }

        Console.Write("Enter New Start Date (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
        {
            Console.WriteLine("Invalid date format.");
            return;
        }

        Console.Write("Enter New End Date (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime endDate) || endDate <= startDate)
        {
            Console.WriteLine("Invalid date format or end date is before the start date.");
            return;
        }

        Console.Write("Enter New Comments: ");
        var comments = Console.ReadLine()?.Trim() ?? string.Empty;

        var updatedExercise = new ExerciseDTO
        {
            StartDate = startDate,
            EndDate = endDate,
            Comments = comments
        };

        var updateResponse = await _httpClient.PutAsJsonAsync($"api/exercises/{exerciseId}", updatedExercise);

        if (updateResponse.IsSuccessStatusCode)
        {
            Console.WriteLine("Exercise updated successfully.");
        }
        else
        {
            Console.WriteLine($"An error occurred: {updateResponse.ReasonPhrase}");
        }
    }


    internal static async Task DeleteExercise()
    {
        Console.Write("Enter Exercise ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int exerciseId))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var response = await _httpClient.DeleteAsync($"api/exercises/{exerciseId}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Exercise deleted successfully.");
        }
        else
        {
            Console.WriteLine($"An error occurred: {response.ReasonPhrase}");
        }
    }

    internal static async Task GetExercises()
    {
        var response = await _httpClient.GetAsync("api/exercises");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var exercises = JsonSerializer.Deserialize<List<ExerciseDTO>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            foreach (var exercise in exercises)
            {
                Console.WriteLine($"Start Date: {exercise.StartDate}");
                Console.WriteLine($"End Date: {exercise.EndDate}");
                Console.WriteLine($"Duration: {exercise.Duration}");
                Console.WriteLine($"Comments: {exercise.Comments}");
                Console.WriteLine("\n---------------------------------------------------------------------------------------------\n");
            }
        }
        else
        {
            Console.WriteLine("An error occurred while fetching exercises.");
        }
    }
}
