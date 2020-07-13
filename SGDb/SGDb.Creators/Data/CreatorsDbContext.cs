using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Data;
using SGDb.Creators.Data.Models;

namespace SGDb.Creators.Data
{
    public class CreatorsDbContext : MessageDbContext
    {
        public CreatorsDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Creator> Creators { get; set; }

        public DbSet<Game> Games { get; set; }
        
        public DbSet<Genre> Genres { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<GamePublisher> GamePublishers { get; set; }

        public DbSet<GameGenre> GameGenres { get; set; }
        
        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();
    }
}