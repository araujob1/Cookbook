using Cookbook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Infrastructure.DataAccess;

internal sealed class CookbookDbContext(DbContextOptions<CookbookDbContext> options) : DbContext(options)
{
    internal DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CookbookDbContext).Assembly);
    }
}
