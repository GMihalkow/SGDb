using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SGDb.Common.Infrastructure.Extensions;

namespace SGDb.Common.Infrastructure.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ValidateImgExtensionsAttribute : ValidationAttribute
    {
        private readonly IEnumerable<string> _allowedImgExtensions;

        /// <summary>
        /// Checks if the provided url is in a valid Image Url format using the provided extensions.
        /// </summary>
        /// <param name="allowedImgExtensions">Comma separated img extensions.</param>
        public ValidateImgExtensionsAttribute(string allowedImgExtensions)
            => this._allowedImgExtensions = allowedImgExtensions
                .Split(new[] {",", ", ", " "}, StringSplitOptions.RemoveEmptyEntries);

        public override bool IsValid(object value)
        {
            var stringVal = value?.ToString() ?? string.Empty;

            this.ErrorMessage = "{0} must be a valid image url using HTTPS. Allowed image formats are jpeg, jpg, png & gif.";
            
            var url = new Uri(stringVal);
            
            return !url.LocalPath.IsNullOrEmpty() && this._allowedImgExtensions.Any(ext => url?.LocalPath.EndsWith(ext) ?? false);
        }
    }
}