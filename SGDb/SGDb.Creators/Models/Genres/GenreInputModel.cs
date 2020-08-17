using System.ComponentModel.DataAnnotations;

namespace SGDb.Creators.Models.Genres
{
    public class GenreInputModel
    {
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        public int CreatorId { get; set; }
    }
}