using Core.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EVN.Domain.Utility
{
    public class HttpRequestUtility
    {
        public static async Task<HttpResponseMessage> Post<T>(string url, T contentValue, Dictionary<string, string> headers = null)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(contentValue.ToJson(), Encoding.UTF8, "application/json");
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        client.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }
                }
                var result = await client.PostAsync(url, content);
                return result;
            }
        }

        public static async Task<T> Get<T>(string url, Dictionary<string, string> headers = null)
        {
            using (var client = new HttpClient())
            {
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        client.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }
                }
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                StringContent content = new StringContent("", Encoding.UTF8, "application/json");
                request.Content = content;
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                string resultContentString = await result.Content.ReadAsStringAsync();
                T resultContent = JsonConvert.DeserializeObject<T>(resultContentString);
                return resultContent;

            }
        }

        public static string GetIpAddress(HttpContext httpContext)
        {
            var ipAddress = string.Empty;
            var ip = httpContext.Connection.RemoteIpAddress;
            if (ip != null)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    ip = Dns.GetHostEntry(ip).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                ipAddress = ip.ToString();
            }
            return ipAddress;
        }
    }
}
