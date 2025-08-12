using ExerciseTrackerUI.Models;
using System.Text;
using System.Text.Json;

namespace ExerciseTrackerUI;

public static class ExerciseHtpp
{
    private static HttpClient httpClient = new HttpClient();

    internal static async Task<List<Exercise>> GetAllExercises()
    {
        try
        {
            string url = "https://localhost:7112/api/exercises";
            HttpResponseMessage res = await httpClient.GetAsync(url);
            res.EnsureSuccessStatusCode();

            string resBody = await res.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<Exercise>>(resBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            throw;
        }
    }

    internal static async Task AddExercise(Exercise exercise)
    {
        try
        {
            string url = "https://localhost:7112/api/exercises";
            string json = JsonSerializer.Serialize(exercise);
            StringContent httpContent = new(json, Encoding.UTF8, "application/json");
            HttpResponseMessage res = await httpClient.PostAsync(url, httpContent);
            res.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }

    internal static async Task UpdateExercise(int id, Exercise exercise)
    {
        try
        {
            string url = $"https://localhost:7112/api/exercises/{id}";
            string json = JsonSerializer.Serialize(exercise);
            StringContent httpContent = new(json, Encoding.UTF8, "application/json");
            HttpResponseMessage res = await httpClient.PutAsync(url, httpContent);
            res.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }

    internal static async Task DeleteExercise(int id)
    {
        try
        {
            string url = $"https://localhost:7112/api/exercises/{id}";
            HttpResponseMessage res = await httpClient.DeleteAsync(url);

            res.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }
}