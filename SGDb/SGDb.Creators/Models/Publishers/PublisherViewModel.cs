using System;

namespace SGDb.Creators.Models.Publishers
{
    public class PublisherViewModel
    {
        public uint Id { get; set; }
        
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public uint CreatorId { get; set; }
    }
}