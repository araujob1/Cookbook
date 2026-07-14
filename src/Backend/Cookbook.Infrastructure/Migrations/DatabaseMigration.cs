using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Infrastructure.Migrations;

public static class DatabaseMigration
{
    public static void MigrateDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();
    }
}
