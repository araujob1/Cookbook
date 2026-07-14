using Cookbook.Domain.Entities;
using Cookbook.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookbook.Infrastructure.DataAccess.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    void IEntityTypeConfiguration<User>.Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(user => user.Name)
            .HasConversion(
                name => name.Value,
                value => new UserName(value))
            .HasMaxLength(100);

        builder.Property(user => user.Email)
            .HasConversion(
                email => email.Value,
                value => new Email(value))
            .HasMaxLength(255);

        builder.Property(user => user.PasswordHash)
            .HasMaxLength(2000);

        builder.HasIndex(user => user.Email)
            .IsUnique()
            .HasFilter("is_active = TRUE");
    }
}
