namespace ECommerce.UI.Interfaces;

public interface IApiService
{
    public Task<string> GetAsync(string path);
    public Task PostAsync<T>(string path, T body);
    public Task DeleteAsync(string path);
}