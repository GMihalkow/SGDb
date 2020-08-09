using System.ComponentModel.DataAnnotations;

namespace SGDb.Creators.Models.Publishers
{
    public class PublisherInputModel
    {
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        public int CreatorId { get; set; }
    }
}