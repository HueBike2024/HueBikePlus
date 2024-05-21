using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Authen.Application.DTOs.Account
{
    public class SignInResponse
    {
        public string AccessToken { get; set; }
        public DateTime ExpiredToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiredRefreshToken { get; set; }
        public string TypeToken { get; set; } = "Bearer";
        public string UrlRedirect { get; set; }
        public bool IsSuperAdmin { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public InfoUser InfoUser { get; set; }
        [JsonIgnore]
        public bool IsFail { get; set; }
        [JsonIgnore]
        public string Message { get; set; }
        [JsonIgnore]
        public int StatusCode { get; set; }

        public SignInResponse()
        {

        }

        public SignInResponse(string msg, int statusCode = StatusCodes.Status200OK, bool isFail = true)
        {
            IsFail = isFail;
            Message = msg;
            StatusCode = statusCode;
        }
    }

    public class InfoUser
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string TimeZone { get; set; }
        public string AvatarPath { get; set; }
    }
}
