using ExerciseTracker.UI.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace ExerciseTracker.UI.Repositories;

public class Repository<T> where T : class
{
    public string BaseUrl { get; set; }
    HttpClientService ClientService = new();
    HttpClient Client;
    public Repository()
    {
        if (typeof(T).Name == "ExerciseShiftDto")
        {
            BaseUrl = ClientService.GetBaseUrl() + "ExerciseShift";
        }
        else
        {
            BaseUrl = ClientService.GetBaseUrl() + typeof(T).Name;
        }
        Client = ClientService.GetHttpClient();
    }

    public async Task<ResponseDto<T>> GetAllEntities()
    {
        try
        {
            using (var response = await Client.GetStreamAsync(BaseUrl))
            {
                ResponseDto<T> GetResponse = JsonSerializer.Deserialize<ResponseDto<T>>(response);
                return GetResponse;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new ResponseDto<T>
            {
                IsSuccess = false,
                Message = e.Message,
                ResponseMethod = "GET",
                Data = []
            };
        }
    }

    public async Task<ResponseDto<T>> GetEntiryById(int? Id)
    {
        try
        {
            using (var response = await Client.GetStreamAsync(BaseUrl + $"/{Id}"))
            {
                ResponseDto<T> GetResponse = JsonSerializer.Deserialize<ResponseDto<T>>(response);
                return GetResponse;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new ResponseDto<T>
            {
                IsSuccess = false,
                Message = e.Message,
                ResponseMethod = "GET",
                Data = []
            };
        }
    }

    public async Task<ResponseDto<T>> CreateEntity(T Entity)
    {
        try
        {
            var response = await Client.PostAsJsonAsync(BaseUrl, Entity);
            using (var StreamResponse = await response.Content.ReadAsStreamAsync())
            {
                ResponseDto<T> Response = await JsonSerializer.DeserializeAsync<ResponseDto<T>>(StreamResponse);
                return Response;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new ResponseDto<T>
            {
                IsSuccess = false,
                Message = e.Message,
                ResponseMethod = "POST",
                Data = []
            };
        }
    }


    public async Task<ResponseDto<T>> UpdateEntity(T Entity, int? Id)
    {
        try
        {
            var response = await Client.PutAsJsonAsync(BaseUrl + $"/{Id}", Entity);
            using (var StreamResponse = await response.Content.ReadAsStreamAsync())
            {
                ResponseDto<T> Response = await JsonSerializer.DeserializeAsync<ResponseDto<T>>(StreamResponse);
                return Response;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new ResponseDto<T>
            {
                IsSuccess = false,
                Message = e.Message,
                ResponseMethod = "PUT",
                Data = []
            };
        }
    }

    public async Task<ResponseDto<T>> DeleteEntity(int? Id)
    {
        try
        {
            var response = await Client.DeleteAsync(BaseUrl + $"/{Id}");
            using (var StreamResponse = await response.Content.ReadAsStreamAsync())
            {
                ResponseDto<T> Response = await JsonSerializer.DeserializeAsync<ResponseDto<T>>(StreamResponse);
                return Response;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new ResponseDto<T>
            {
                IsSuccess = false,
                Message = e.Message,
                ResponseMethod = "DELETE",
                Data = []
            };
        }
    }
}
