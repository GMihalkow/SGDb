namespace SGDb.Server.Data.Models
{
    public class GameGenre
    {
        public uint GameId { get; set; }

        public Game Game { get; set; }

        public uint GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}