using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDb.Creators.Data.Models;

namespace SGDb.Creators.Data.EntityConfiguration
{
    public class CreatorConfiguration : IEntityTypeConfiguration<Creator>
    {
        public void Configure(EntityTypeBuilder<Creator> builder)
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
                .HasMany(u => u.Publishers)
                .WithOne(g => g.Creator)
                .HasForeignKey(g => g.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}