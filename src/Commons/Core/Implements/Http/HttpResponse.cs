using Core.Interfaces.Http;
using Newtonsoft.Json;
using System.Net;

namespace Core.Implements.Http
{
    public class HttpResponse : IHttpResponse
    {
        public int StatusCode { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
        public Metadata Paging { get; set; }

        private static HttpResponse Instance(object data
            , Metadata metaData
            , string message
            , HttpStatusCode statusCode)
        {
            return new HttpResponse()
            {
                StatusCode = (int)statusCode,
                Data = data,
                Paging = metaData,
                Message = message
            };
        }

        public static HttpResponse Ok(object data = null, Metadata metaData = null
            , string message = null
            , HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return Instance(data, metaData, message, statusCode);
        }

        public static HttpResponse Error(string message = null
            , HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return Instance(null, null, message, statusCode);
        }
    }

    public class HttpResponse<T> : HttpResponse
    {
        private static HttpResponse<T> Instance(object data
            , Metadata metaData
            , string message
            , HttpStatusCode statusCode)
        {
            return new HttpResponse<T>()
            {
                StatusCode = (int)statusCode,
                Data = data,
                Paging = metaData,
                Message = message
            };
        }

        public static HttpResponse<T> Ok(T data = default(T)
            , Metadata metaData = null
            , string message = null
            , HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return Instance(data, metaData, message, statusCode);
        }

        public new static HttpResponse<T> Error(string message = null
            , HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return Instance(null, null, message, statusCode);
        }
    }

    public class Metadata
    {
        [JsonProperty(PropertyName = "pageIndex")]
        public int PageIndex { get; set; }

        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; }

        [JsonProperty(PropertyName = "totalRecords")]
        public long TotalCount { get; set; }
        [JsonProperty(PropertyName = "totalPage")]
        public int TotalPage => (int)(TotalCount / PageSize) + (TotalCount % PageSize > 0 ? 1 : 0);
    }
}