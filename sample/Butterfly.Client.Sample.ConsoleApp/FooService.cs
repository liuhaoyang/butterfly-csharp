using System.Net.Http;

namespace Butterfly.Client.Sample.ConsoleApp
{
    public class FooService : IFooService
    {
        private readonly HttpClient _httpClient;

        public FooService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public string GetValues()
        {
            return _httpClient.GetStringAsync("http://localhost:5002/api/values").Result;
        }
    }
}