using Core.Models.Base;
using Core.Models.File;
using Core.Models.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Net;
using static Core.Common.AppConstants;

namespace Infrastructure.Services
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
        Task<BaseMedia> UploadStreamFile(Stream fileStream, string fileName, string savePath = "", bool isUploadImageThumbnail = false, int thumbnailWidth = 300, bool isUploadVideo = false);
        Task<BaseMedia> UploadFile(IFormFile file, string savePath = "", bool isUploadImageThumbnail = false, int thumbnailWidth = 300, bool isUploadVideo = false);
        /// <summary>
        /// Delete One file by path
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        bool DeleteFile(string fullFilePath);

        Task<List<BaseMedia>> UploadFiles(IFormFile[] files, string savePath = "", bool isUploadImageThumbnail = false, int thumbnailWidth = 300, bool isUploadVideo = false);

        void ResizeImage(IFormFile file, string fullFilePath = "", int thumbnailWidth = 300);
    }

    public class FileService : IFileService
    {
        private readonly Core.Infrastructure.Services.IFileService _fileService;
        private readonly MediaSetting _mediaSetting;

        public FileService(Core.Infrastructure.Services.IFileService fileService, IOptions<MediaSetting> mediaSetting)
        {
            _fileService = fileService;
            _mediaSetting = mediaSetting.Value;
        }

        public bool DeleteFile(string fullFilePath)
        {
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), fullFilePath);

            if (File.Exists(pathToSave))
            {
                File.Delete(pathToSave);
            }
            return true;
        }

        public async Task<BaseMedia> UploadFile(IFormFile file, string savePath = "", bool isUploadImageThumbnail = false, int thumbnailWidth = 300, bool isUploadVideo = false)
        {
            List<string> extensions = new() { ".jpeg", ".png", ".jfif", ".jpg", ".svg", ".tiff" };
            var folderName = savePath;
            if (string.IsNullOrEmpty(savePath))
                folderName = Path.Combine(_mediaSetting.Folders.Root, _mediaSetting.Folders.Files);

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length <= ContentTypes.MAX_FILE_SIZE)
            {
                var fileNameUpload = $"{DateTime.UtcNow.ToString(DateTimeFormat.FILE_YYMMDDHHMMSS)}_{Guid.NewGuid()}_{file.FileName.Replace(' ', '_')}";
                var fileNameThumbnailUpload = $"{DateTime.UtcNow.ToString(DateTimeFormat.FILE_YYMMDDHHMMSS)}_{Guid.NewGuid()}_Thumbnail_{file.FileName.Replace(' ', '_')}";
                var extension = Path.GetExtension(file.FileName);
                if (string.IsNullOrEmpty(extension))
                {
                    extension = file.ContentType.Split('/')[1];
                }
                new FileExtensionContentTypeProvider().TryGetContentType(file.FileName, out var contentType);
                contentType = contentType ?? ContentTypes.APPLICATION_OCTECT_STREAM;


                var mediaPath = Path.Combine(pathToSave, fileNameUpload);
                var dbPath = Path.Combine(folderName, fileNameUpload);

                var mediaThumbnailPath = Path.Combine(pathToSave, fileNameThumbnailUpload);
                var dbThumbnailPath = Path.Combine(folderName, fileNameThumbnailUpload);

                _fileService.UploadFile(new FileModel
                {
                    FileName = fileNameUpload.IndexOf('.') == -1 ? $"{fileNameUpload}.{extension}" : fileNameUpload,
                    File = file
                }, mediaPath);

                var media = new BaseMedia(file.FileName, fileNameUpload, (int)HttpStatusCode.OK, extension, contentType, $"{dbPath.Replace("\\", "/")}");
                // upload thumbnail
                if (isUploadImageThumbnail && extensions.Contains(extension))
                {
                    ResizeImage(file, mediaThumbnailPath, thumbnailWidth);
                    media.FilePathThumbnail = $"{dbThumbnailPath.Replace("\\", "/")}";
                }

                // upload thumbnail video
                //if (isUploadVideo)
                //{
                //    List<string> videoExtensions = new() { ".mp4", ".mov", ".avi", ".wmv", ".mpeg-2", ".mkv" };
                //    if (videoExtensions.Contains(extension))
                //    {
                //        var thumbnailVideoPath = $"{mediaThumbnailPath[..mediaThumbnailPath.LastIndexOf('.')]}.png";
                //        var dbThumbnailVideoPath = $"{dbThumbnailPath[..dbThumbnailPath.LastIndexOf('.')]}.png";
                //        UploadThumbnailVideo(mediaPath, thumbnailVideoPath);
                //        media.FilePathThumbnail = $"{dbThumbnailVideoPath.Replace("\\", "/")}";
                //    }
                //}               
                return await Task.FromResult(media);
            }
            return null;
        }

        public async Task<List<BaseMedia>> UploadFiles(IFormFile[] files, string savePath = "", bool isUploadImageThumbnail = false, int thumbnailWidth = 300, bool isUploadVideo = false)
        {
            List<BaseMedia> medias = new();

            if (files.Length == 0)
                return medias;

            foreach (var file in files)
            {
                var media = await UploadFile(file, savePath, isUploadImageThumbnail, thumbnailWidth, isUploadVideo);
                if (media != null)
                    medias.Add(media);
            }
            return medias;
        }
        public void ResizeImage(IFormFile file, string fullFilePath = "", int thumbnailWidth = 300)
        {
            using var image = Image.Load(file.OpenReadStream());
            var divisor = image.Width / thumbnailWidth;

            if (divisor == 0)
            {
                image.Save(fullFilePath);
                return;
            }

            var thumbnailHeight = Convert.ToInt32(Math.Round((decimal)(image.Height / divisor)));
            //var thumbnailHeight = height + Convert.ToInt32(Math.Round((decimal)((thumbnailWidth - height) * 0.5)));

            image.Mutate(x => x.Resize(thumbnailWidth, thumbnailHeight, KnownResamplers.Lanczos2));
            image.Save(fullFilePath);
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

        public Task<BaseMedia> UploadStreamFile(Stream fileStream, string fileName, string savePath = "", bool isUploadImageThumbnail = false, int thumbnailWidth = 300, bool isUploadVideo = false)
        {
            List<string> extensions = new() { ".jpeg", ".png", ".jfif", ".jpg", ".svg", ".tiff" };
            var folderName = savePath;
            if (string.IsNullOrEmpty(savePath))
                folderName = Path.Combine(_mediaSetting.Folders.Root, _mediaSetting.Folders.Files);

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            // Convert stream to IFormFile
            IFormFile formFile = new FormFile(fileStream, 0, fileStream.Length, fileName, fileName);

            if (formFile.Length <= ContentTypes.MAX_FILE_SIZE)
            {
                var fileNameUpload = $"{DateTime.UtcNow.ToString(DateTimeFormat.FILE_YYMMDDHHMMSS)}_{Guid.NewGuid()}_{formFile.FileName.Replace(' ', '_')}";
                var fileNameThumbnailUpload = $"{DateTime.UtcNow.ToString(DateTimeFormat.FILE_YYMMDDHHMMSS)}_{Guid.NewGuid()}_Thumbnail_{formFile.FileName.Replace(' ', '_')}";
                var extension = Path.GetExtension(formFile.FileName);
                if (string.IsNullOrEmpty(extension))
                {
                    extension = formFile.ContentType.Split('/')[1];
                }
                new FileExtensionContentTypeProvider().TryGetContentType(formFile.FileName, out var contentType);
                contentType = contentType ?? ContentTypes.APPLICATION_OCTECT_STREAM;


                var mediaPath = Path.Combine(pathToSave, fileNameUpload);
                var dbPath = Path.Combine(folderName, fileNameUpload);

                var mediaThumbnailPath = Path.Combine(pathToSave, fileNameThumbnailUpload);
                var dbThumbnailPath = Path.Combine(folderName, fileNameThumbnailUpload);

                _fileService.UploadFile(new FileModel
                {
                    FileName = fileNameUpload.IndexOf('.') == -1 ? $"{fileNameUpload}.{extension}" : fileNameUpload,
                    File = formFile
                }, mediaPath);

                var media = new BaseMedia(formFile.FileName, fileNameUpload, (int)HttpStatusCode.OK, extension, contentType, $"{dbPath.Replace("\\", "/")}");
                // upload thumbnail
                if (isUploadImageThumbnail && extensions.Contains(extension))
                {
                    ResizeImage(formFile, mediaThumbnailPath, thumbnailWidth);
                    media.FilePathThumbnail = $"{dbThumbnailPath.Replace("\\", "/")}";
                }

                // upload thumbnail video
                //if (isUploadVideo)
                //{
                //    List<string> videoExtensions = new() { ".mp4", ".mov", ".avi", ".wmv", ".mpeg-2", ".mkv" };
                //    if (videoExtensions.Contains(extension))
                //    {
                //        var thumbnailVideoPath = $"{mediaThumbnailPath[..mediaThumbnailPath.LastIndexOf('.')]}.png";
                //        var dbThumbnailVideoPath = $"{dbThumbnailPath[..dbThumbnailPath.LastIndexOf('.')]}.png";
                //        UploadThumbnailVideo(mediaPath, thumbnailVideoPath);
                //        media.FilePathThumbnail = $"{dbThumbnailVideoPath.Replace("\\", "/")}";
                //    }
                //}               
                return Task.FromResult(media);
            }
            return null;
        }
    }
}
