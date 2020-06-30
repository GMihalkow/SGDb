using Microsoft.EntityFrameworkCore;
using SGDb.Statistics.Data.EntityConfiguration;
using SGDb.Statistics.Data.Models;

namespace SGDb.Statistics.Data
{
    public class StatisticsDbContext : DbContext
    {
        public StatisticsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Models.Statistics> Statistics { get; set; }

        public DbSet<GameDetailsView> GameDetailsViews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new GameDetailsViewConfiguration());
        }
    }
}