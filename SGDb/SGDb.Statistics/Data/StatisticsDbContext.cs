using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Data;
using SGDb.Statistics.Data.Models;

namespace SGDb.Statistics.Data
{
    public class StatisticsDbContext : MessageDbContext
    {
        public StatisticsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Models.Statistics> Statistics { get; set; }

        public DbSet<GameDetailsView> GameDetailsViews { get; set; }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();
    }
}