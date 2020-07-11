using System;

namespace SGDb.Creators.Models.Genres
{
    public class GenreViewModel
    {
        public uint Id { get; set; }
        
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public uint CreatorId { get; set; }
    }
}