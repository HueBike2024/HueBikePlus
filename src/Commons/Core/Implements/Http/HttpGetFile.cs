using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.Implements.Http
{
    public class HttpGetFile : BaseHttpFile
    {
        public override List<KeyValuePair<string, string>> Headers
        {
            get
            {
                return new List<KeyValuePair<string, string>>()
                 {
                    new KeyValuePair<string,string>("keep-alive", "true"),
                    new KeyValuePair<string,string>("Accept-Encoding", "gzip"),
                    new KeyValuePair<string,string>("Accept-Encoding", "deflate"),
                 };
            }
        }

        public override async Task<HttpResponseMessage> SendAsync(byte[] fileContent, string fileName, string host)
        {
            InstanceHeaders(Headers);
            return await HttpClient.GetAsync(host);
        }
    }
}
