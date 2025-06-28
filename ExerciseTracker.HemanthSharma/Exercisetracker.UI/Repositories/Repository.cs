using ExerciseTracker.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExerciseTracker.UI.Repositories
{
    public class Repository<T> where T : class
    {
        public string BaseURL { get; set; }
        HttpClientService ClientService = new();
        HttpClient Client;
        public Repository()
        {
            if (typeof(T).Name == "ExerciseShiftDto")
            {
                BaseURL = ClientService.GetBaseURL() + "ExerciseShift";
            }
            else
            {
                BaseURL = ClientService.GetBaseURL() + typeof(T).Name;
            }
            Client = ClientService.GetHttpClient();
        }
        public async Task<ResponseDto<T>> GetAllEntities()
        {
            try
            {
                //https://localhost:7249/api/Exercise
                //https://localhost:7249/api/ExerciseShift
                string Stringresponse = await Client.GetStringAsync(BaseURL);
                using (var response = await Client.GetStreamAsync(BaseURL))
                {
                    ResponseDto<T> GetResponse = JsonSerializer.Deserialize<ResponseDto<T>>(response);
                    return GetResponse;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return new ResponseDto<T>
                {
                  IsSuccess=false,
                  Message=e.Message,
                  ResponseMethod="GET",
                  Data = []
                };
            }
        }
        public async Task<ResponseDto<T>> GetEntiryById(int? Id)
        {
            try
            {
                using (var response = await Client.GetStreamAsync(BaseURL + $"/{Id}"))
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
                var response = await Client.PostAsJsonAsync(BaseURL, Entity);
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
                var response = await Client.PutAsJsonAsync(BaseURL + $"/{Id}", Entity);
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
                var response = await Client.DeleteAsync(BaseURL + $"/{Id}");
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
}
