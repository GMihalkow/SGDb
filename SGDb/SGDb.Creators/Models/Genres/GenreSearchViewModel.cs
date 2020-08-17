using System;

namespace SGDb.Creators.Models.Genres
{
    public class GenreSearchViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatorId { get; set; }

        public string CreatorName { get; set; }
    }
}