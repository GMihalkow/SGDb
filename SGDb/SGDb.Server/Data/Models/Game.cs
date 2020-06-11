using System;
using System.Collections.Generic;

namespace SGDb.Server.Data.Models
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
        
        public string CreatorId { get; set; }

        public User Creator { get; set; }
        
        public ICollection<GameGenre> Genres { get; set; }
        
        public ICollection<GamePublisher> Publishers { get; set; }
    }
}