namespace SGDb.Creators.Data.Models
{
    public class GamePublisher
    {
        public uint GameId { get; set; }

        public Game Game { get; set; }

        public uint PublisherId { get; set; }

        public Publisher Publisher { get; set; }
    }
}