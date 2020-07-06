using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGDb.Identity.Data.Models;

namespace SGDb.Identity.Data.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(DataConstants.Users.UsernameMaxLength);

            builder
                .HasIndex(u => u.UserName)
                .IsUnique();

            builder
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(DataConstants.Users.EmailAddressMaxLength);

            builder
                .HasIndex(u => u.Email)
                .IsUnique();
            
            builder
                .Property(u => u.PasswordHash)
                .IsRequired();
        }
    }
}