using System.ComponentModel.DataAnnotations;

namespace SGDb.Identity.Models.Identity
{
    public class LoginInputModel
    {
        [Required]
        [MinLength(6)]
        [MaxLength(70)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}