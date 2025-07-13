using Newtonsoft.Json;
using System.Text;
using ExerciseTrackerUI.Models;

namespace ExerciseTrackerUI.Services;

public class ExerciseService
{
  private readonly HttpClient _httpClient;
  private readonly string _baseUrl;

  public ExerciseService()
  {
    _httpClient = new HttpClient();
    _baseUrl = "http://localhost:5239/api";
  }

  public async Task<List<Exercise>> GetAllExercisesAsync()
  {
    try
    {
      var response = await _httpClient.GetAsync($"{_baseUrl}/exercise");

      if (response.IsSuccessStatusCode)
      {
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<Exercise>>(json) ?? new List<Exercise>();
      }
      else
      {
        Console.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
        return new List<Exercise>();
      }
    }
    catch (HttpRequestException ex)
    {
      Console.WriteLine($"Network error: {ex.Message}");
      return new List<Exercise>();
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Unexpected error: {ex.Message}");
      return new List<Exercise>();
    }
  }

  public async Task<Exercise?> GetExerciseByIdAsync(int id)
  {
    try
    {
      var response = await _httpClient.GetAsync($"{_baseUrl}/exercise/{id}");

      if (response.IsSuccessStatusCode)
      {
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Exercise>(json);
      }
      else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
      {
        return null;
      }
      else
      {
        Console.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
        return null;
      }
    }
    catch (HttpRequestException ex)
    {
      Console.WriteLine($"Network error: {ex.Message}");
      return null;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Unexpected error: {ex.Message}");
      return null;
    }
  }

  public async Task<bool> CreateExerciseAsync(Exercise exercise)
  {
    try
    {
      var json = JsonConvert.SerializeObject(exercise);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      
      Console.WriteLine($"Sending request to: {_baseUrl}/exercise");
      Console.WriteLine($"Request body: {json}");
      
      var response = await _httpClient.PostAsync($"{_baseUrl}/exercise", content);
      
      Console.WriteLine($"Response status: {response.StatusCode}");
      Console.WriteLine($"Response content: {await response.Content.ReadAsStringAsync()}");

      return response.IsSuccessStatusCode;
    }
    catch (HttpRequestException ex)
    {
      Console.WriteLine($"Network error: {ex.Message}");
      return false;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Unexpected error: {ex.Message}");
      return false;
    }
  }

  public async Task<bool> UpdateExerciseAsync(Exercise exercise)
  {
    try
    {
      var json = JsonConvert.SerializeObject(exercise);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      var response = await _httpClient.PutAsync($"{_baseUrl}/exercise/{exercise.Id}", content);

      return response.IsSuccessStatusCode;
    }
    catch (HttpRequestException ex)
    {
      Console.WriteLine($"Network error: {ex.Message}");
      return false;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Unexpected error: {ex.Message}");
      return false;
    }
  }

  public async Task<bool> DeleteExerciseAsync(int id)
  {
    try
    {
      var response = await _httpClient.DeleteAsync($"{_baseUrl}/exercise/{id}");

      return response.IsSuccessStatusCode;
    }
    catch (HttpRequestException ex)
    {
      Console.WriteLine($"Network error: {ex.Message}");
      return false;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Unexpected error: {ex.Message}");
      return false;
    }
  }

  public void Dispose()
  {
    _httpClient?.Dispose();
  }
}