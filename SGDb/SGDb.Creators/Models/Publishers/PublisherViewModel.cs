using System;

namespace SGDb.Creators.Models.Publishers
{
    public class PublisherViewModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatorId { get; set; }
    }
}