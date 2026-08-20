using Cookbook.Domain.Repositories;
using Cookbook.Domain.Repositories.User;
using Cookbook.Domain.Security.PasswordHashing;
using Cookbook.Domain.Security.Tokens;
using Cookbook.Infrastructure.DataAccess;
using Cookbook.Infrastructure.DataAccess.Repositories;
using Cookbook.Infrastructure.Migrations;
using Cookbook.Infrastructure.Security.PasswordHashing;
using Cookbook.Infrastructure.Security.Tokens;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Infrastructure.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        AddDbContext(services);
        AddMigrationRunner(services);
        AddPasswordHasher(services);
        AddRepositories(services);
        AddSecurity(services);

        return services;
    }

    private static void AddDbContext(IServiceCollection services)
    {
        services.AddDbContext<CookbookDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            options
                .UseNpgsql(configuration.GetDbConnectionString())
                .UseSnakeCaseNamingConvention();
        });
    }

    private static void AddMigrationRunner(IServiceCollection services)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(serviceProvider =>
                    serviceProvider
                        .GetRequiredService<IConfiguration>()
                        .GetDbConnectionString())
                .ScanIn(typeof(DatabaseVersions).Assembly).For.Migrations())
            .AddLogging(logging => logging.AddFluentMigratorConsole());
    }

    private static void AddPasswordHasher(IServiceCollection services) =>
        services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
    }

    private static void AddSecurity(IServiceCollection services)
    {
        services.AddScoped<IAccessTokenGenerator>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();

            var expirationTimeMinutes = configuration.GetValue<uint>("Jwt:ExpirationTimeMinutes");
            var signingKey = configuration.GetValue<string>("Jwt:SigningKey")!;

            return new JwtTokenHandler(expirationTimeMinutes, signingKey);
        });
    }
}
