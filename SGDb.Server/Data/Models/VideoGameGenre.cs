namespace SGDb.Server.Data.Models
{
    public class VideoGameGenre
    {
        public uint GameId { get; set; }

        public VideoGame Game { get; set; }

        public uint GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}