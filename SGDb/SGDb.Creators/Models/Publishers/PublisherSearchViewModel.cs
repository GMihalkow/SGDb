using System;

namespace SGDb.Creators.Models.Publishers
{
    public class PublisherSearchViewModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatorName { get; set; }
    }
}