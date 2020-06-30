using System;

namespace SGDb.Creators.Gateway.Models.Creators
{
    public class CreatorViewModel
    {
        public uint Id { get; set; }

        public string Username { get; set; }

        public DateTime CreatedOn { get; set; }

        public int TotalGamesCreatedCount { get; set; }
    }
}