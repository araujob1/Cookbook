using Cookbook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Integrations.Tests")]
namespace Cookbook.Infrastructure.DataAccess;

internal sealed class CookbookDbContext(DbContextOptions<CookbookDbContext> options) : DbContext(options)
{
    internal DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CookbookDbContext).Assembly);
    }
}
