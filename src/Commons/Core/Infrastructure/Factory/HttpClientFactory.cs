using Microsoft.Extensions.Configuration;

namespace Core.Infrastructure.Factory
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient();
    }
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly IConfiguration _configuration;
        public HttpClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public HttpClient CreateClient()
        {
            var timeout = Convert.ToInt32(_configuration["ConnectTimeout"]);
            var client = new HttpClient();
            SetupClientDefaults(client, timeout);
            return client;
        }

        protected virtual void SetupClientDefaults(HttpClient client, int timeout)
        {
            client.Timeout = TimeSpan.FromSeconds(timeout);
            client.MaxResponseContentBufferSize = int.MaxValue;
        }
    }
}
