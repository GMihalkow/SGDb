using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Data.EntityConfiguration;
using SGDb.Common.Data.Models;

namespace SGDb.Common.Data
{
    public abstract class MessageDbContext : DbContext
    {
        protected MessageDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }

        protected abstract Assembly ConfigurationsAssembly { get; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MessageConfiguration());
            builder.ApplyConfigurationsFromAssembly(this.ConfigurationsAssembly);
            
            base.OnModelCreating(builder);
        }
    }
}