using Core.Exceptions;
using Core.Helpers;
using Core.Models;
using Core.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Core.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private string[] _extensions;
        public AllowedExtensionsAttribute()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            var settings = configuration.GetSection("Extensions").Get<AllowExtensions>();
            _extensions = settings.ImageExtension;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            var file = value as IFormFile;
            if (file == null)
            {
                throw new BaseException("Định dạng file không phù hợp");
            }
            var extension = Path.GetExtension(file.FileName);
            if (file != null)
            {
                if (!_extensions.Contains(extension.ToLower()))
                {
                    throw new BaseException("Định dạng file không phù hợp");
                }
            }

            return ValidationResult.Success;
        }
    }
}
