using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SGDb.Server.Data.Models
{
    public class User : IdentityUser
    {   
        public ICollection<Genre> Genres { get; set; }

        public ICollection<VideoGame> Games { get; set; }
        
        public ICollection<Publisher> Publishers { get; set; }
    }
}