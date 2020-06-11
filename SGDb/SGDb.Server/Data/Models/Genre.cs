using System;
using System.Collections.Generic;

namespace SGDb.Server.Data.Models
{
    public class Genre : BaseEntity<uint>
    {
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public string CreatorId { get; set; }

        public User Creator { get; set; }
        
        public ICollection<GameGenre> Games { get; set; }
    }
}