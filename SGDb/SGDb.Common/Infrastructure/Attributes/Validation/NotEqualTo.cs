using System;
using System.ComponentModel.DataAnnotations;
using SGDb.Common.Infrastructure.Extensions;

namespace SGDb.Common.Infrastructure.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotEqualTo : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public NotEqualTo(string otherPropertyName, string errorMessage = null)
        {
            this._otherPropertyName = otherPropertyName;
            this.ErrorMessage = errorMessage;
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {   
                var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(this._otherPropertyName);
                if (otherProperty == null)
                    throw new InvalidOperationException($"Property with name [{this._otherPropertyName}] not found.");
                
                var otherPropertyValue = otherProperty?.GetValue(validationContext.ObjectInstance);

                if (value.Equals(otherPropertyValue))
                {
                    var finalErrorMessage = this.ErrorMessage.IsNullOrEmpty()
                        ? this.ErrorMessage
                        : string.Format("The value of {0} cannot be the same as the value of the {1}.",
                            validationContext.DisplayName, otherProperty.Name);
                    
                    return new ValidationResult(finalErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}