internal static class HttpClientFactory
{
    private static readonly HttpClient _httpClient;

    static HttpClientFactory()
    {
        _httpClient = new HttpClient();
    }

    internal static HttpClient GetHttpClient()
    {
        return _httpClient;
    }
}
