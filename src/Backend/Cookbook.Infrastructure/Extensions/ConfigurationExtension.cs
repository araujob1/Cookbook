using Microsoft.Extensions.Configuration;

namespace Cookbook.Infrastructure.Extensions;

internal static class ConfigurationExtension
{
    internal static string GetDbConnectionString(this IConfiguration configuration) =>
        configuration.GetConnectionString("DefaultConnection")!;
}
