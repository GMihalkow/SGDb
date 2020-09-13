using System.ComponentModel.DataAnnotations;

namespace SGDb.Identity.Models.Roles
{
    public class RoleInputModel
    {
        [Required]
        [MinLength(4)]
        [MaxLength(256)]
        public string Name { get; set; }
    }
}