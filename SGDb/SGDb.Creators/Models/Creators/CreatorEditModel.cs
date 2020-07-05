using System.ComponentModel.DataAnnotations;

namespace SGDb.Creators.Models.Creators
{
    public class CreatorEditModel
    {
        [Required]
        public uint Id { get; set; }
        
        [Required]
        public string Username { get; set; }
    }
}