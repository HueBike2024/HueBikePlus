using Core.Implements.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;

namespace Core.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BaseApiController : ControllerBase
    {
        [NonAction]
        public virtual IActionResult JsonResponse(HttpResponse response, int statusCode = (int)HttpStatusCode.OK)
        {
            return StatusCode(statusCode, new { statusCode = response.StatusCode, response.Data, response.Paging, response.Message });
        }
        protected HttpResponseMessage FileDownload(byte[] fileContent, string fileName, string fileType)
        {
            var responseMsg = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileContent)
            };
            responseMsg.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            responseMsg.Content.Headers.ContentType = new MediaTypeHeaderValue(fileType);
            return responseMsg;
        }
    }
}
