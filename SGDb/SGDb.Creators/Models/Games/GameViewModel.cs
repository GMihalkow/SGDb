using System;

namespace SGDb.Creators.Models.Games
{
    public class GameViewModel
    {
        public string Name { get; set; }

        public string HeaderImageUrl { get; set; }

        public string BackgroundImageUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public string About { get; set; }

        public decimal? Price { get; set; }

        public uint? Recommendations { get; set; }

        public int CreatorId { get; set; }

        public DateTime? ReleasedOn { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}