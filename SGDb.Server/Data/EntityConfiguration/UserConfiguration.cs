using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDb.Server.Data.Models;

namespace SGDb.Server.Data.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder
                .HasMany(u => u.Games)
                .WithOne(g => g.Creator)
                .HasForeignKey(g => g.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasMany(u => u.Genres)
                .WithOne(g => g.Creator)
                .HasForeignKey(g => g.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasMany(u => u.Studios)
                .WithOne(g => g.Creator)
                .HasForeignKey(g => g.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasMany(u => u.Publishers)
                .WithOne(g => g.Creator)
                .HasForeignKey(g => g.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}