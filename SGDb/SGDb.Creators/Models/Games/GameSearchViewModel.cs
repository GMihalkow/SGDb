using System;
using System.Collections.Generic;

namespace SGDb.Creators.Models.Games
{
    public class GameSearchViewModel
    {
        public uint Id { get; set; }

        public string Name { get; set; }
        
        public string About { get; set; }

        public string BackgroundImageUrl { get; set; }
        
        public string HeaderImageUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public decimal? Price { get; set; }

        public uint? Recommendations { get; set; }

        public DateTime? ReleasedOn { get; set; }
        
        public IEnumerable<uint> PublisherIds { get; set; }

        public IEnumerable<uint> GenreIds { get; set; }
    }
}