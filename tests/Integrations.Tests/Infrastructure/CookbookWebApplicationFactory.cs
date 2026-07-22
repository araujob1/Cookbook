using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace Integrations.Tests.Infrastructure;

public sealed class CookbookWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _databaseContainer = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("cookbook")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public async ValueTask InitializeAsync()
    {
        await _databaseContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _databaseContainer.GetConnectionString()
            });
        });
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _databaseContainer.DisposeAsync();
    }
}
