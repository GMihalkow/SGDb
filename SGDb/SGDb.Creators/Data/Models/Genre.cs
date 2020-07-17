using System;
using System.Collections.Generic;
using SGDb.Common.Data.Models;

namespace SGDb.Creators.Data.Models
{
    public class Genre : BaseEntity<int>
    {
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public int CreatorId { get; set; }

        public Creator Creator { get; set; }
        
        public ICollection<GameGenre> Games { get; set; }
    }
}