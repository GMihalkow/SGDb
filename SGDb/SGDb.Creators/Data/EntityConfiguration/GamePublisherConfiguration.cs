using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDb.Creators.Data.Models;

namespace SGDb.Creators.Data.EntityConfiguration
{
    public class GamePublisherConfiguration : IEntityTypeConfiguration<GamePublisher>
    {
        public void Configure(EntityTypeBuilder<GamePublisher> builder)
        {
            builder.HasOne(vgp => vgp.Game)
                .WithMany(g => g.Publishers)
                .HasForeignKey(vgs => vgs.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(vgp => vgp.Publisher)
                .WithMany(g => g.Games)
                .HasForeignKey(vgs => vgs.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasKey(vgs => new {vgs.GameId, vgs.PublisherId});
        }
    }
}