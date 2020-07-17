using SGDb.Common.Data.Models;

namespace SGDb.Statistics.Data.Models
{
    public class GameDetailsView : BaseEntity<int>
    {
        public string UserId { get; set; }

        public int GameId { get; set; }
    }
}