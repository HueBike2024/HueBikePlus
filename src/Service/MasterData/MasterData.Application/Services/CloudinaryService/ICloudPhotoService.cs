using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterData.Application.DTOs.CloudPhoto;
using Core.Exceptions;
using GoogleApi.Entities.Maps.Common;

namespace MasterData.Application.Services.CloudinaryService
{
    public interface ICloudPhotoService
    {
        /// <summary>
        /// Uploadfile lên và trả về đường link
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<CloudPhotoResponse> UploadPhotoAsync(IFormFile file, string typeOfPhoto);

        /// <summary>
        /// Xóa ảnh
        /// </summary>
        /// <param name="publicId"></param>
        /// <returns></returns>
        Task<DeleteCloudPhotoResponse> DeletePhotoAsync(string publicId);

        /// <summary>
        /// Upload Base64
        /// </summary>
        /// <param name="base64Photo"></param>
        /// <param name="typeOfPhoto"></param>
        /// <returns></returns>
        Task<CloudPhotoResponse> UploadPhotoFromBase64Async(string base64Photo, string typeOfPhoto);
    }

    public class CloudPhotoService : ICloudPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public CloudPhotoService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary ?? throw new ArgumentNullException(nameof(cloudinary));
        }

        public async Task<DeleteCloudPhotoResponse> DeletePhotoAsync(string publicId)
        {
            var deletionResult = await _cloudinary.DestroyAsync(new DeletionParams(publicId));

            return new DeleteCloudPhotoResponse
            {
                IsSuccess = deletionResult.Result == "ok",
            };
        }

        public async Task<CloudPhotoResponse> UploadPhotoAsync(IFormFile file, string typeOfPhoto)
        {
            // Checks if the cloudinary account is configured
            if (string.IsNullOrEmpty(_cloudinary.Api.Account.Cloud) ||
                string.IsNullOrEmpty(_cloudinary.Api.Account.ApiKey) ||
                string.IsNullOrEmpty(_cloudinary.Api.Account.ApiSecret))
            {
                throw new BaseException("Không tìm thấy tài khoản truy cập Cloudinary");
            }

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                if (file.Length > 5242880)
                {
                    throw new BaseException("Kích thước của ảnh không được vượt quá 5mb");
                }
                using var stream = file.OpenReadStream(); //using so it disposes it from memory

                Transformation transformation = new()
                {
                };

                //different photo configurations for different types of services
                if (typeOfPhoto == "profile")
                {
                    transformation = new Transformation()
                        .Height(400)
                        .Width(400)
                        .Crop("fill")
                        .Gravity("face")
                        .Quality("auto:best")
                        .FetchFormat("auto");
                }
                else if (typeOfPhoto == "notification" || typeOfPhoto == "information" || typeOfPhoto == "post" || typeOfPhoto == "banner")
                {
                    transformation = new Transformation()
                        .Crop("fill")
                        .Gravity("face")
                        .Quality("auto:best")
                        .FetchFormat("auto");
                }

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = transformation,
                    Folder = "huebike-images"
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            var result = new CloudPhotoResponse
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString(),
            };

            return result;
        }

        public async Task<CloudPhotoResponse> UploadPhotoFromBase64Async(string base64Photo, string typeOfPhoto)
        {
            if (string.IsNullOrEmpty(_cloudinary.Api.Account.Cloud) ||
        string.IsNullOrEmpty(_cloudinary.Api.Account.ApiKey) ||
        string.IsNullOrEmpty(_cloudinary.Api.Account.ApiSecret))
            {
                throw new BaseException("Không tìm thấy tài khoản truy cập Cloudinary");
            }

            var uploadResult = new ImageUploadResult();

            if (!string.IsNullOrWhiteSpace(base64Photo))
            {
                var fileBytes = Convert.FromBase64String(base64Photo);
                if (fileBytes.Length > 5242880)
                {
                    throw new BaseException("Kích thước của ảnh không được vượt quá 5mb");
                }

                using var stream = new MemoryStream(fileBytes);

                var transformation = GetTransformation(typeOfPhoto);

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription("base64Photo", stream),
                    Transformation = transformation,
                    Folder = "huebike-images"
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            var result = new CloudPhotoResponse
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString(),
            };

            return result;
        }

        private Transformation GetTransformation(string typeOfPhoto)
        {
            return new Transformation()
            .Height(200)
            .Width(200)
            .Crop("fill")
            .Gravity("face")
            .Quality("auto:best")
            .FetchFormat("auto");
        }
    }
}
