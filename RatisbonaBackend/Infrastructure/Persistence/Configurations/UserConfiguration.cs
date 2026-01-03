using RatisbonaBackend.Business.Entities;

namespace RatisbonaBackend.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table
        builder.ToTable("users");

        // Primary Key
        builder.HasKey(u => u.Id);

        // Properties
        builder.Property(u => u.Id)
            .IsRequired();

        builder.Property(u => u.Firstname)
            .IsRequired();

        builder.Property(u => u.Lastname)
            .IsRequired();

        builder.Property(u => u.Email)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("now()");

        builder.Property(u => u.UpdatedAt)
            .IsRequired(false);

        builder.Property(u => u.LastLoginAt)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(u => u.Email)
            .IsUnique();
    }
}
