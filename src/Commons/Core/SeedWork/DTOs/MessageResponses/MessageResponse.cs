using Newtonsoft.Json;

namespace Core.SeedWork.DTOs.MessageResponses
{
    public static class MessageResponse
    {
        public static string ToJsonString(string key = "", string value = "")
        {
            var msg = new { key = key, value = value };
            return $"[{JsonConvert.SerializeObject(msg)}]";
        }
    }
}
