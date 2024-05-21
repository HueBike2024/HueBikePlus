using Core.Models.File;
using Core.Models.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;

namespace Core.Infrastructure.Services
{
    public interface IFileService
    {
        /// <summary>
        /// Upload file
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void UploadFile(FileModel request, string mediaUploadFolder);
        /// <summary>
        /// Upload list files
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        void UploadFiles(List<FileModel> requests, string mediaUploadFolder);
    }

    public class FileService : IFileService
    {
        private readonly MediaSetting _options;
        public FileService(IOptions<MediaSetting> options)
        {
            _options = options.Value;
        }

        public void UploadFile(FileModel request, string mediaUploadFolder)
        {
            using FileStream stream = new FileStream(mediaUploadFolder, FileMode.Create, FileAccess.ReadWrite);
            request.File.CopyTo(stream);
        }

        public void UploadFiles(List<FileModel> requests, string mediaUploadFolder)
        {
            foreach (var request in requests)
            {
                UploadFile(request, mediaUploadFolder);
            }
        }
    }
}
