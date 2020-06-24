using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDb.Creators.Data.Models;

namespace SGDb.Creators.Data.EntityConfiguration
{
    public class CreatorConfiguration : IEntityTypeConfiguration<Creator>
    {
        public void Configure(EntityTypeBuilder<Creator> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.HasIndex(c => c.Username).IsUnique();

            builder.Property(c => c.Username)
                .IsRequired()
                .HasMaxLength(DataConstants.Creators.UsernameMaxLength);

            builder.Property(c => c.UserId).IsRequired();

            builder
                .HasMany(c => c.Games)
                .WithOne(g => g.Creator)
                .HasForeignKey(g => g.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasMany(c => c.Genres)
                .WithOne(g => g.Creator)
                .HasForeignKey(g => g.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(c => c.Publishers)
                .WithOne(g => g.Creator)
                .HasForeignKey(g => g.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}