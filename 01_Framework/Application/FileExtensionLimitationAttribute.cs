using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace _01_Framework.Application
{
    public class FileExtensionLimitationAttribute : ValidationAttribute , IClientModelValidator
    {
        private readonly string[] _extensions;

        public FileExtensionLimitationAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        public override bool IsValid(object value)
        {
            var file = value as IFormFile;
            if (file == null) return true;
            var fileExtension = Path.GetExtension(file.FileName);
            return _extensions.Contains(fileExtension);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            //context.Attributes.Add("data-val", "true");
            //context.Attributes.Add("data-val-fileExtensionLimitation", ErrorMessage);
        }
    }
}
