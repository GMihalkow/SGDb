using System;

namespace SGDb.Creators.Models.Creators
{
    public class CreatorViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public DateTime CreatedOn { get; set; }

        public int TotalGamesCreatedCount { get; set; }
    }
}