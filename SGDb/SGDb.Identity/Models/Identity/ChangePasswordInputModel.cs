using SGDb.Common.Infrastructure.Attributes.Validation;
using System.ComponentModel.DataAnnotations;

namespace SGDb.Identity.Models.Identity
{
    public class ChangePasswordInputModel
    {
        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        
        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [NotEqualTo(nameof(CurrentPassword))]
        public string NewPassword { get; set; }
    }
}