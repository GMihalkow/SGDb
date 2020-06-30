using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDb.Statistics.Data.Models;

namespace SGDb.Statistics.Data.EntityConfiguration
{
    public class GameDetailsViewConfiguration : IEntityTypeConfiguration<GameDetailsView>
    {
        public void Configure(EntityTypeBuilder<GameDetailsView> builder)
        {
            builder.HasKey(gdv => gdv.Id);

            builder.Property(gdv => gdv.Id).ValueGeneratedOnAdd();
            
            builder.Property(gdv => gdv.GameId).IsRequired();
            builder.Property(gdv => gdv.UserId).IsRequired();
        }
    }
}