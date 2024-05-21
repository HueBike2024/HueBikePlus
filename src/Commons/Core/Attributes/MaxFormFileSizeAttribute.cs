using Core.Exceptions;
using Core.Helpers;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EVN.Domain.Attributes
{
    public class MaxFormFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize = 50000000;
        public MaxFormFileSizeAttribute()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            _maxFileSize = Convert.ToInt32(configuration["FileMaxSize"]);
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
                throw new BaseException("File không tồn tại");
            }
            if (file.Length > _maxFileSize)
            {
                throw new BaseException("File size không phù hợp, tối đa 10MB/ file");
            }

            return ValidationResult.Success;
        }
    }
}
