using Microsoft.EntityFrameworkCore;
using SGDb.Creators.Data.EntityConfiguration;
using SGDb.Creators.Data.Models;

namespace SGDb.Creators.Data
{
    public class CreatorsDbContext : DbContext
    {
        public CreatorsDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Creator> Creators { get; set; }

        public DbSet<Game> Games { get; set; }
        
        public DbSet<Genre> Genres { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<GamePublisher> GamePublishers { get; set; }

        public DbSet<GameGenre> GameGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //TODO [GM]: Make some Video Game columns not required (price, website)
        
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new PublisherConfiguration());
            builder.ApplyConfiguration(new GenreConfiguration());
            builder.ApplyConfiguration(new GameConfiguration());
            builder.ApplyConfiguration(new CreatorConfiguration());
            builder.ApplyConfiguration(new GameGenreConfiguration());
            builder.ApplyConfiguration(new GamePublisherConfiguration());
        }
    }
}