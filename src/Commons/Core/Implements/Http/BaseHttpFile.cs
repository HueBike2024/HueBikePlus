using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Core.Implements.Http
{
    public abstract class BaseHttpFile : IDisposable
    {
        public abstract List<KeyValuePair<string, string>> Headers { get; }

        public BaseHttpFile()
        {
            HttpClient = new HttpClient();
        }

        public void Dispose()
        {
            HttpClient.Dispose();
        }

        public virtual void AddCredential(string userName, string password)
        {
            var basicAuthorization = Encoding.ASCII.GetBytes($"{userName}:{password}");
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(basicAuthorization));
        }

        public HttpClient HttpClient { get; set; }

        public abstract Task<HttpResponseMessage> SendAsync(byte[] fileContent, string fileName, string host);

        public void InstanceHeaders(List<KeyValuePair<string, string>> headers)
        {
            foreach (var header in headers)
            {
                HttpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }
}
