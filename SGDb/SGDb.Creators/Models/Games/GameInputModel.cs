using System;

namespace SGDb.Creators.Models.Games
{
    public class GameInputModel
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
    }
}