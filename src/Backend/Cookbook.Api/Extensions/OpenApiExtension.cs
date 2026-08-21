using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;

namespace Cookbook.Api.Extensions;

public static class OpenApiExtension
{
    public static IServiceCollection AddApplicationOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Enter the JWT access token without the 'Bearer' prefix."
                };

                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??=
                    new Dictionary<string, IOpenApiSecurityScheme>();
                document.Components.SecuritySchemes.Add(
                    JwtBearerDefaults.AuthenticationScheme,
                    securityScheme);

                return Task.CompletedTask;
            });

            options.AddOperationTransformer((operation, context, _) =>
            {
                var endpointMetadata = context.Description.ActionDescriptor.EndpointMetadata;
                var requiresAuthorization = endpointMetadata.OfType<IAuthorizeData>().Any();
                var allowsAnonymous = endpointMetadata.OfType<IAllowAnonymous>().Any();

                if (requiresAuthorization && allowsAnonymous == false)
                {
                    operation.Security ??= [];
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference(
                            JwtBearerDefaults.AuthenticationScheme,
                            context.Document)] = []
                    });
                }

                return Task.CompletedTask;
            });
        });

        return services;
    }
}
