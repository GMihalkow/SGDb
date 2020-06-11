using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SGDb.Server.Data.EntityConfiguration;
using SGDb.Server.Data.Models;

namespace SGDb.Server.Data
{
    public class SGDbContext : IdentityDbContext<User>
    {
        public SGDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Game> Games { get; set; }
        
        public DbSet<Genre> Genres { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<GamePublisher> GamePublishers { get; set; }

        public DbSet<GameGenre> GameGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //TODO [GM]: Make some Video Game columns not required (price, website)
        
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new GameGenreConfiguration());
            builder.ApplyConfiguration(new GamePublisherConfiguration());
        }
    }
}