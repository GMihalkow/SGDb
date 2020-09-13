using System.ComponentModel.DataAnnotations;

namespace SGDb.Identity.Models.Roles
{
    public class RoleEditModel : RoleInputModel
    {
        [Required]
        public string Id { get; set; }
    }
}