using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ExerciseTracker.ASV.Models;

namespace ExerciseTracker.ASV.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly HttpClient _httpClient;

    public ExerciseRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
    public async Task<bool> DeleteExercise(int id)
    {
        var url = $"https://localhost:7133/api/ExerciseDatas/{id}";
        var response = await  _httpClient.DeleteAsync(url);
        if (response.IsSuccessStatusCode)
            return true;
        return false;
    }

    public async Task<bool> PutExercise(ExerciseData exerciseData)
    {
        try
        {
            var url = $"https://localhost:7133/api/ExerciseDatas/{exerciseData.Id}";
            var jsonContent = new StringContent(JsonSerializer.Serialize(exerciseData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, jsonContent);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<ExerciseData> GetExerciseById(int id)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var url = $"https://localhost:7133/api/ExerciseDatas/{id}";
            string exerciseDataJson = await _httpClient.GetStringAsync(url);
            ExerciseData exerciseData = JsonSerializer.Deserialize<ExerciseData>(exerciseDataJson, options) ?? new ExerciseData();
            return exerciseData;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return new ExerciseData();
    }

    public async Task<List<ExerciseData>> GetExercises()
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            string exerciseDataJson = await _httpClient.GetStringAsync("https://localhost:7133/api/ExerciseDatas/");
            List<ExerciseData> exerciseDatas = JsonSerializer.Deserialize<List<ExerciseData>>(exerciseDataJson, options) ?? new List<ExerciseData>();
            return exerciseDatas;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return new List<ExerciseData>();
    }

    public async Task<bool> PostExercise(ExerciseData exerciseData)
    {
        try
        {
            var url = "https://localhost:7133/api/ExerciseDatas";
            var jsonContent = new StringContent(JsonSerializer.Serialize(exerciseData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, jsonContent);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}