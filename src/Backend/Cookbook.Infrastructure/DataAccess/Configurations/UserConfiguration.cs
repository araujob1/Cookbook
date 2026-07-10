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
                value => new UserName(value));

        builder.Property(user => user.Email)
            .HasConversion(
                email => email.Value,
                value => new Email(value));

        builder.HasIndex(user => user.Email)
            .IsUnique()
            .HasFilter("is_active = TRUE");
    }
}
