using System.ComponentModel.DataAnnotations;
using SGDb.Common.Infrastructure.Attributes.Validation;

namespace SGDb.Identity.Models.Identity
{
    public class ChangePasswordInputModel
    {
        [Required]
        public string CurrentPassword { get; set; }
        
        [Required]
        [NotEqualTo(nameof(CurrentPassword))]
        public string NewPassword { get; set; }
    }
}