using System.Net.Http.Json;
using ECommerce.UI.Configuration;
using ECommerce.UI.Interfaces;

namespace ECommerce.UI.Services;

public class ApiService(IHttpClientFactory clientFactory) : IApiService
{
    private static readonly string BaseUrl = ApiSettings.BaseUrl;

    public Task<string> GetAsync(string path)
    {
        return SendGetRequest(path);
    }


    public Task PostAsync<T>(string path, T body)
    {
        return SendPostRequest(path, body);
    }

    public async Task DeleteAsync(string path)
    {
        await SendDeleteRequest(path);
    }


    //------- Helper Methods -------
    private async Task<string> SendGetRequest(string path)
    {
        var client = clientFactory.CreateClient(BaseUrl);
        var response = await client.GetAsync(path);

        return await HandleResponse(response);
    }

    private async Task SendPostRequest<T>(string path, T body)
    {
        var client = clientFactory.CreateClient(BaseUrl);
        var response = await client.PostAsJsonAsync(path, body);
        await HandleResponse(response);
    }

    private async Task SendDeleteRequest(string path)
    {
        var client = clientFactory.CreateClient(BaseUrl);
        var response = await client.DeleteAsync(path);

        await HandleResponse(response);
    }

    private static async Task<string> HandleResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

        switch ((int)response.StatusCode)
        {
            case var code when code >= 400 && code < 500:
                throw new HttpRequestException($"Client side error occurred, status code: {code}", null,
                    response.StatusCode);

            case var code when code >= 500:
                throw new HttpRequestException($"Server side error occurred, status code: {code}", null,
                    response.StatusCode);

            default:
                throw new HttpRequestException(
                    $"An unexpected error occurred with the status code: {response.StatusCode}",
                    null, response.StatusCode);
        }
    }
}