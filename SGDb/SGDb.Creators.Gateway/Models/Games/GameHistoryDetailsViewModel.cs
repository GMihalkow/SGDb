using System;

namespace SGDb.Creators.Gateway.Models.Games
{
    public class GameHistoryDetailsViewModel
    {
        public int GameId { get; set; }

        public SimpleGameViewModel Game { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}