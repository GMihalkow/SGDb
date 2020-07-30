using System;
using System.Collections.Generic;

namespace SGDb.Creators.Gateway.Models.Games
{
    public class GameSearchViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string About { get; set; }

        public string BackgroundImageUrl { get; set; }
        
        public string HeaderImageUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public decimal? Price { get; set; }

        public DateTime? ReleasedOn { get; set; }

        public int CreatorId { get; set; }
        
        public IEnumerable<int> PublisherIds { get; set; }

        public IEnumerable<int> GenreIds { get; set; }
        public uint Views { get; set; }
    }
}