using Newtonsoft.Json;

namespace Core.Interfaces.Http
{
    public interface IHttpResponse
    {
        [JsonProperty(PropertyName = "statusCode")]
        [JsonIgnore]
        public int StatusCode { get; set; }
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}