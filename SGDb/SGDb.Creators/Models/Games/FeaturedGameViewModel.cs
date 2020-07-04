using System;

namespace SGDb.Creators.Models.Games
{
    public class FeaturedGameViewModel
    {
        public uint Id { get; set; }
        
        public string Name { get; set; }

        public string PublisherNames { get; set; }

        public string GenreNames { get; set; }

        public string HeaderImageUrl { get; set; }
        
        public DateTime? ReleasedOn { get; set; }
    }
}