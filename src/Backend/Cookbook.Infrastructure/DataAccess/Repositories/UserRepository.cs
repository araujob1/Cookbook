using Cookbook.Domain.Entities;
using Cookbook.Domain.Repositories.User;
using Cookbook.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository(CookbookDbContext dbContext) : IUserReadOnlyRepository,
    IUserWriteOnlyRepository
{
    public async Task AddAsync(User user) =>
        await dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmailAsync(Email email) =>
        await dbContext.Users.AnyAsync(x => x.IsActive && x.Email == email);
}
