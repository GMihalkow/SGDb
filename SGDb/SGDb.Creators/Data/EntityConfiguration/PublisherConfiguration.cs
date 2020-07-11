using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDb.Creators.Data.Models;
using System;

namespace SGDb.Creators.Data.EntityConfiguration
{
    public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.CreatedOn)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .Property(p => p.CreatorId)
                .IsRequired();
            
            builder
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(p => p.Name)
                .HasMaxLength(DataConstants.Publishers.MaxNameLength)
                .IsRequired();
        }
    }
}