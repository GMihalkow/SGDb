using SGDb.Common.Data.Models;

namespace SGDb.Statistics.Data.Models
{
    public class GameDetailsView : BaseEntity<uint>
    {
        public string UserId { get; set; }

        public uint GameId { get; set; }
    }
}