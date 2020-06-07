using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDb.Server.Data.Models;

namespace SGDb.Server.Data.EntityConfiguration
{
    public class VideoGamePublisherConfiguration : IEntityTypeConfiguration<VideoGamePublisher>
    {
        public void Configure(EntityTypeBuilder<VideoGamePublisher> builder)
        {
            builder.HasOne(vgp => vgp.Game)
                .WithMany(g => g.Publishers)
                .HasForeignKey(vgs => vgs.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(vgp => vgp.Publisher)
                .WithMany(g => g.Games)
                .HasForeignKey(vgs => vgs.PublisherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasKey(vgs => new {vgs.GameId, vgs.PublisherId});
        }
    }
}