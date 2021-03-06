using System.ComponentModel.DataAnnotations;

namespace SGDb.Identity.Models.Identity
{
    public class RegisterInputModel
    {
        [Required]
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
         [Required] 
         [EmailAddress]
         [MinLength(6)]
         [MaxLength(70)]
         public string EmailAddress { get; set; }
         
         [Required] 
         [Phone]
         [MinLength(10)]
         [MaxLength(12)]
         public string PhoneNumber { get; set; }

         [Required]
         [DataType(DataType.Password)]
         [MinLength(6)]
         [MaxLength(50)]
         public string Password { get; set; }
         
         [Required]
         [DataType(DataType.Password)]
         [Compare(nameof(Password))]
         [MinLength(6)]
         [MaxLength(50)]
         public string ConfirmPassword { get; set; }
    }
}