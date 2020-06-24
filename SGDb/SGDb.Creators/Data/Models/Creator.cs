using System.Collections.Generic;

namespace SGDb.Creators.Data.Models
{
    public class Creator
    {
        public uint Id { get; set; }

        public string Username { get; set; }
        
        public string UserId { get; set; }
        
        public ICollection<Genre> Genres { get; set; }

        public ICollection<Game> Games { get; set; }
        
        public ICollection<Publisher> Publishers { get; set; }
    }
}