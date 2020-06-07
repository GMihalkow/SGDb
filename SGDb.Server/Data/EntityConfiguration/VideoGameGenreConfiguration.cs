using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDb.Server.Data.Models;

namespace SGDb.Server.Data.EntityConfiguration
{
    public class VideoGameGenreConfiguration : IEntityTypeConfiguration<VideoGameGenre>
    {
        public void Configure(EntityTypeBuilder<VideoGameGenre> builder)
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