using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Cookbook.Api.Extensions;

public static class LocalizationExtension
{
    public static IServiceCollection AddApplicationLocalization(this IServiceCollection services)
    {
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new("en"),
                new("pt-BR"),
            };

            options.DefaultRequestCulture = new RequestCulture("en");

            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            options.RequestCultureProviders =
            [
                new AcceptLanguageHeaderRequestCultureProvider()
            ];
        });

        return services;
    }

    public static WebApplication UseApplicationLocalization(this WebApplication app)
    {
        var localizationOptions = app
            .Services
            .GetRequiredService<IOptions<RequestLocalizationOptions>>();

        app.UseRequestLocalization(localizationOptions.Value);

        return app;
    }
}
