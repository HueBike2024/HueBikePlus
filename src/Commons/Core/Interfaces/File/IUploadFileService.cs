using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Core.Interfaces.File
{
    public interface IUploadFileService
    {
        /// <summary>
        /// Updload file from IFormFile
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<string> Upload(IFormFile file, string filePath);
        /// <summary>
        /// Updload file from byte[]
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<string> Upload(byte[] bytes, string filePath);
        /// <summary>
        /// Updload file from MemoryStream
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<string> Upload(MemoryStream stream, string filePath);
        /// <summary>
        /// Delete One file by path
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<bool> DeleteFile(string fullFilePath);
        /// <summary>
        /// Delete all file in folder
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        Task<bool> DeleteFolder(string folderPath);
    }
}
