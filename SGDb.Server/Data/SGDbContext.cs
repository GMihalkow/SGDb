using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SGDb.Server.Data.EntityConfiguration;
using SGDb.Server.Data.Models;

namespace SGDb.Server.Data
{
    public class SGDbContext : IdentityDbContext<User>
    {
        public SGDbContext(DbContextOptions options) : base(options) { }

        public DbSet<VideoGame> Games { get; set; }
        
        public DbSet<Genre> Genres { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Studio> Studios { get; set; }
        
        public DbSet<VideoGameStudio> VideoGameStudios { get; set; }

        public DbSet<VideoGamePublisher> VideoGamePublishers { get; set; }

        public DbSet<VideoGameGenre> VideoGameGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new VideoGameGenreConfiguration());
            builder.ApplyConfiguration(new VideoGamePublisherConfiguration());
            builder.ApplyConfiguration(new VideoGameStudioConfiguration());
        }
    }
}