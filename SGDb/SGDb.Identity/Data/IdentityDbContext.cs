using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Data.EntityConfiguration;
using SGDb.Common.Data.Models;
using SGDb.Identity.Data.EntityConfiguration;
using SGDb.Identity.Data.Models;

namespace SGDb.Identity.Data
{
    public class IdentityDbContext : IdentityDbContext<User>
    {
        public IdentityDbContext (DbContextOptions options) : base(options) { }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MessageConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            
            base.OnModelCreating(builder);
        }
    }
}