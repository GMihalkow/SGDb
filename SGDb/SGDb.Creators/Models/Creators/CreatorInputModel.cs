using System.ComponentModel.DataAnnotations;

namespace SGDb.Creators.Models.Creators
{
    public class CreatorInputModel
    {
        [Required]
        public string Username { get; set; }
        
        public string UserId { get; set; }
    }
}