using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Base
{
    public class BaseMedia : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }
        [Required]
        [StringLength(255)]
        public string FileNameUpload { get; set; }
        public int StatusCode { get; set; }
        [Required]
        [StringLength(128)]
        public string Extension { get; set; }
        [Required]
        [StringLength(128)]
        public string Type { get; set; }
        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }

        public string FilePathThumbnail { get; set; }
        public BaseMedia()
        {

        }
        public BaseMedia(string fileName, string fileNameUpload, int statusCode, string extension, string type, string filePath, string filePathThumbnail = null)
        {            
            FileName = fileName;
            FileNameUpload = fileNameUpload;
            StatusCode = statusCode;
            Extension = extension;
            Type = type;
            FilePath = filePath;
            FilePathThumbnail = filePathThumbnail;
        }
    }
}
