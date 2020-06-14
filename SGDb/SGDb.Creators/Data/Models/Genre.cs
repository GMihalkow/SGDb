using System;
using System.Collections.Generic;

namespace SGDb.Creators.Data.Models
{
    public class Genre : BaseEntity<uint>
    {
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public uint CreatorId { get; set; }

        public Creator Creator { get; set; }
        
        public ICollection<GameGenre> Games { get; set; }
    }
}