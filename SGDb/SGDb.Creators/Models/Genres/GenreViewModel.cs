using System;

namespace SGDb.Creators.Models.Genres
{
    public class GenreViewModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public int CreatorId { get; set; }
    }
}