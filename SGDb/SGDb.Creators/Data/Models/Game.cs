using System;
using System.Collections.Generic;
using SGDb.Common.Data.Models;

namespace SGDb.Creators.Data.Models
{
    public class Game : BaseEntity<uint>
    {
        public string Name { get; set; }

        public string HeaderImageUrl { get; set; }

        public string BackgroundImageUrl { get; set; }

        public string Description { get; set; }

        public string WebsiteUrl { get; set; }

        public string About { get; set; }

        public decimal? Price { get; set; }

        public uint? Recommendations { get; set; }

        public DateTime? ReleasedOn { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public uint CreatorId { get; set; }

        public Creator Creator { get; set; }
        
        public ICollection<GameGenre> Genres { get; set; }
        
        public ICollection<GamePublisher> Publishers { get; set; }
    }
}