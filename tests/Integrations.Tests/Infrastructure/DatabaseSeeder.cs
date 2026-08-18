using CommonTestUtilities.Entities;
using Cookbook.Domain.Security.PasswordHashing;
using Cookbook.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using UserEntity = Cookbook.Domain.Entities.User;

namespace Integrations.Tests.Infrastructure;

public sealed record SeededUser(UserEntity Entity, string Password);

public sealed record SeededUsers(SeededUser User1);

public sealed record DatabaseSeed(SeededUsers Users);

internal sealed class DatabaseSeeder(IServiceProvider serviceProvider)
{
    public async Task<DatabaseSeed> SeedAsync()
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<CookbookDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        var (user, password) = UserBuilder.Build();

        user.PasswordHash = passwordHasher.HashPassword(password);

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var seededUser1 = new SeededUser(user, password);

        return new DatabaseSeed(new SeededUsers(seededUser1));
    }
}
