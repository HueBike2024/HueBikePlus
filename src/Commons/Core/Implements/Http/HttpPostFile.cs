using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Core.Implements.Http
{
    public class HttpPostFile : BaseHttpFile
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

        private ContentDispositionHeaderValue DispositionHeaderValue(string fileName)
        {
            return new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"file\"",
                FileName = "\"" + fileName + "\""
            };
        }

        public override async Task<HttpResponseMessage> SendAsync(byte[] fileContent, string fileName, string host)
        {
            InstanceHeaders(Headers);
            using (var memStream = new MemoryStream(fileContent))
            {
                string boundary = Guid.NewGuid().ToString();
                var streamContent = new StreamContent(memStream, (int)memStream.Length);

                streamContent.Headers.ContentDisposition = DispositionHeaderValue(fileName);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                var content = new MultipartFormDataContent(boundary);
                content.Headers.Remove("Content-Type");
                content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);
                content.Add(streamContent);

                return await HttpClient.PostAsync(host, content);
            }
        }
    }
}
