using Cookbook.Domain.Repositories;
using Cookbook.Domain.Repositories.User;
using Cookbook.Domain.Security.PasswordHashing;
using Cookbook.Infrastructure.DataAccess;
using Cookbook.Infrastructure.DataAccess.Repositories;
using Cookbook.Infrastructure.Migrations;
using Cookbook.Infrastructure.Security.PasswordHashing;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Infrastructure.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddMigrationRunner(services, configuration);
        AddPasswordHasher(services);
        AddRepositories(services);

        return services;
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetDbConnectionString();

        services.AddDbContext<CookbookDbContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });
    }

    private static void AddMigrationRunner(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetDbConnectionString();

        services.AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(DatabaseVersions).Assembly).For.Migrations())
            .AddLogging(logging => logging.AddFluentMigratorConsole());
    }

    private static void AddPasswordHasher(this IServiceCollection services) =>
        services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
    }
}
