using System.ComponentModel.DataAnnotations;

namespace SGDb.Identity.Models.Identity
{
    public class LoginInputModel
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}