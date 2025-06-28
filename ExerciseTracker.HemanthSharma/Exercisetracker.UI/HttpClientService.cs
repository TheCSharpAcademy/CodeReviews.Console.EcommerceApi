using System.Configuration;

namespace ExerciseTracker.UI
{
    public class HttpClientService
    {
        private string BaseUrl { get; set; }
        public HttpClientService()
        {
            BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];
        }
        public string GetBaseURL()
        {
            return BaseUrl;
        }
        public HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            return client;
        }
    }
}
