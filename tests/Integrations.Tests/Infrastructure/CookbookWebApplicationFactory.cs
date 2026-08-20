using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace Integrations.Tests.Infrastructure;

public sealed class CookbookWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private DatabaseSeed? _seed;

    private readonly PostgreSqlContainer _databaseContainer = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("cookbook")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public DatabaseSeed Seed => _seed
        ?? throw new InvalidOperationException("The database seed has not been initialized.");

    public async ValueTask InitializeAsync()
    {
        await _databaseContainer.StartAsync();

        var databaseSeeder = new DatabaseSeeder(Services);
        _seed = await databaseSeeder.SeedAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _databaseContainer.GetConnectionString(),
                ["Jwt:ExpirationTimeMinutes"] = "1000",
                ["Jwt:SigningKey"] = "giKjBYhx756Hk0xhtD2BSl3lRKbvboVQ"
            });
        });
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _databaseContainer.DisposeAsync();
    }
}
