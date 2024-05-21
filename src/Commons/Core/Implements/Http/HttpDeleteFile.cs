using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.Implements.Http
{
    public class HttpDeleteFile : BaseHttpFile
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

            string boundary = Guid.NewGuid().ToString();

            var contents = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("selection",fileName),
                new KeyValuePair<string, string>("action","delete"),
            };
            var formContent = new FormUrlEncodedContent(contents);
            formContent.Headers.Remove("Content-Type");
            formContent.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

            //content.Add(formContent);

            return await HttpClient.PostAsync(host, formContent);
        }
    }
}
