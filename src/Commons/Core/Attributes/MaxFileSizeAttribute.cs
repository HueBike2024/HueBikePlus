using Core.Exceptions;
using Core.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            _maxFileSize = Convert.ToInt32(configuration["ImageMaxSize"]);
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file == null)
            {
                throw new BaseException("File không tồn tại");
            }
            if (file.Length > _maxFileSize)
            {
                throw new BaseException("File size không phù hợp. Tối đa 10MB/file");
            }

            return ValidationResult.Success;
        }
    }
}
