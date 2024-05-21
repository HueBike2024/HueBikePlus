using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Core.Models
{
    public class JsonResponse
    {
        [JsonProperty(PropertyName = "statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }

        [JsonProperty(PropertyName = "developerMessage")]
        public object DeveloperMessage { get; set; }

    }

    public class JsonResponse<T>
    {
        [JsonProperty(PropertyName = "statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }

        [JsonProperty(PropertyName = "developerMessage")]
        public object DeveloperMessage { get; set; }

        /** Full optional constructor */
        public JsonResponse(int statusCode = 200, string message = "", string developerMessage = "", int? pageIndex = null, T data = default(T))
        {
            StatusCode = statusCode;
            Message = message;
            DeveloperMessage = developerMessage;
            Data = data;
        }

        public static JsonResponse<T> Ok(T data)
        {
            return new JsonResponse<T>(StatusCodes.Status200OK, data: data);
        }
    }
}
