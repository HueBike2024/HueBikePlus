using System;

namespace Core.Models.File
{
    public class FileResponse
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string FileNameUpload { get; set; }
        public int StatusCode { get; set; }
        public string Extension { get; set; }
        public string Type { get; set; }
        public string FilePath { get; set; }
    }
}
