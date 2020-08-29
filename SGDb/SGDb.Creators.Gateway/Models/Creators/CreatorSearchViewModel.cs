using System;

namespace SGDb.Creators.Gateway.Models.Creators
{
    public class CreatorSearchViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public DateTime CreatedOn { get; set; }

        public int TotalGamesCreatedCount { get; set; }

        public string RoleName { get; set; }
    }
}