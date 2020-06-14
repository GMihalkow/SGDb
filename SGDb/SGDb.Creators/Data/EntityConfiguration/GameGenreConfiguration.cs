using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDb.Creators.Data.Models;

namespace SGDb.Creators.Data.EntityConfiguration
{
    public class GameGenreConfiguration : IEntityTypeConfiguration<GameGenre>
    {
        public void Configure(EntityTypeBuilder<GameGenre> builder)
        {
            builder.HasOne(vgg => vgg.Game)
                .WithMany(g => g.Genres)
                .HasForeignKey(vgs => vgs.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(vgg => vgg.Genre)
                .WithMany(g => g.Games)
                .HasForeignKey(vgs => vgs.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasKey(vgs => new {vgs.GameId, vgs.GenreId});
        }
    }
}