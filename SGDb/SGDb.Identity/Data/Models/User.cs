using Microsoft.AspNetCore.Identity;

namespace SGDb.Identity.Data.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}