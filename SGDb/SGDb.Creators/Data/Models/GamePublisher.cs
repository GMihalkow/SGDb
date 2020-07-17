namespace SGDb.Creators.Data.Models
{
    public class GamePublisher
    {
        public int GameId { get; set; }

        public Game Game { get; set; }

        public int PublisherId { get; set; }

        public Publisher Publisher { get; set; }
    }
}