using System.ComponentModel.DataAnnotations;

namespace SGDb.Creators.Models.Creators
{
    public class CreatorEditModel
    {
        [Required]
        public uint Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
    }
}