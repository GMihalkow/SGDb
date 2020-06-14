using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDb.Server.Data.Models;

namespace SGDb.Server.Data.EntityConfiguration
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.CreatorId).IsRequired();
            builder.Property(g => g.CreatedOn).IsRequired();
            builder.Property(g => g.Id).ValueGeneratedOnAdd();
            builder.Property(g => g.HeaderImageUrl).IsRequired();
            builder.Property(g => g.BackgroundImageUrl).IsRequired();
            
            builder
                .Property(g => g.Price)
                .HasColumnType("decimal(18,2)");

            builder
                .Property(g => g.Description)
                .HasMaxLength(DataConstants.Games.MaxDescriptionLength);
            
            builder
                .Property(g => g.About)
                .HasMaxLength(DataConstants.Games.MaxAboutLength);
            
            builder
                .Property(g => g.Name)
                .HasMaxLength(DataConstants.Games.MaxNameLength)
                .IsRequired();
        }
    }
}