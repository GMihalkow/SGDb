using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDb.Common.Data.Models;

namespace SGDb.Common.Data.EntityConfiguration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property<string>("serializedData")
                .IsRequired()
                .HasField("serializedData");

            builder
                .Property(m => m.GuidId)
                .IsRequired();
            
            builder
                .HasIndex(m => m.GuidId)
                .IsUnique();

            builder
                .Property(m => m.CreatedOn)
                .ValueGeneratedOnAdd()
                .IsRequired();
            
            builder
                .Property(m => m.Type)
                .IsRequired()
                .HasConversion(
                    t => t.AssemblyQualifiedName,
                    t => Type.GetType(t));
        }
    }
}