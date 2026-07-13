using Cookbook.Domain.Repositories;

namespace Cookbook.Infrastructure.DataAccess.Repositories;

internal sealed class UnitOfWork(CookbookDbContext dbContext) : IUnitOfWork
{
    public async Task CommitAsync() => await dbContext.SaveChangesAsync();
}
