using System;

namespace SGDb.Creators.Gateway.Models.Games
{
    public class GameDetailsViewModel
    {
        public string Name { get; set; }

        public string HeaderImageUrl { get; set; }

        public string BackgroundImageUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public string About { get; set; }

        public decimal? Price { get; set; }

        public DateTime? ReleasedOn { get; set; }

        public uint Views { get; set; }
    }
}