using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDb.Server.Data.Models;

namespace SGDb.Server.Data.EntityConfiguration
{
    public class VideoGameStudioConfiguration : IEntityTypeConfiguration<VideoGameStudio>
    {
        public void Configure(EntityTypeBuilder<VideoGameStudio> builder)
        {
            builder.HasOne(vgs => vgs.Game)
                .WithMany(g => g.Studios)
                .HasForeignKey(vgs => vgs.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(vgs => vgs.Studio)
                .WithMany(g => g.Games)
                .HasForeignKey(vgs => vgs.StudioId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasKey(vgs => new {vgs.GameId, vgs.StudioId});
        }
    }
}