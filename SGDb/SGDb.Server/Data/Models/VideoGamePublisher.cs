namespace SGDb.Server.Data.Models
{
    public class VideoGamePublisher
    {
        public uint GameId { get; set; }

        public VideoGame Game { get; set; }

        public uint PublisherId { get; set; }

        public Publisher Publisher { get; set; }
    }
}