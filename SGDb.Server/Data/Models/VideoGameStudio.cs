namespace SGDb.Server.Data.Models
{
    public class VideoGameStudio
    {
        public uint GameId { get; set; }

        public VideoGame Game { get; set; }

        public uint StudioId { get; set; }

        public Studio Studio { get; set; }
    }
}