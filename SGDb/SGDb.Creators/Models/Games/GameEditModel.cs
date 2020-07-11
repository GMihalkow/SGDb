using System.ComponentModel.DataAnnotations;

namespace SGDb.Creators.Models.Games
{
    public class GameEditModel : GameInputModel
    {
        [Required]
        public uint Id { get; set; }
    }
}