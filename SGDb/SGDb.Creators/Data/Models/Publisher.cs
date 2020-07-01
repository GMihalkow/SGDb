using System;
using System.Collections.Generic;
using SGDb.Common.Data.Models;

namespace SGDb.Creators.Data.Models
{
    public class Publisher : BaseEntity<uint>
    {
        public string Name { get; set; }

        public string Description { get; set; }
        
        public DateTime CreatedOn { get; set; }

        public uint CreatorId { get; set; }

        public Creator Creator { get; set; }
        
        public ICollection<GamePublisher> Games { get; set; }
    }
}