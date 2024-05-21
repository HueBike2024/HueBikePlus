using Microsoft.AspNetCore.Http;

namespace Core.Models.File
{
    public class FileModel
    {
        public string FileName { get; set; }
        public IFormFile File { get; set; }
        public string Base64Data { get; set; }
    }
}
