using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDb.Creators.Data.Models;

namespace SGDb.Creators.Data.EntityConfiguration
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(g => g.Id);

            builder
                .Property(g => g.CreatedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .Property(g => g.CreatorId)
                .IsRequired();
            
            builder
                .Property(g => g.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(g => g.Name)
                .HasMaxLength(DataConstants.Genres.MaxNameLength)
                .IsRequired();

            builder
                .HasIndex(g => g.Name)
                .IsUnique();
        }
    }
}