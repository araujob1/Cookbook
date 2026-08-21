using Cookbook.Communication.Responses;
using Cookbook.Domain.Repositories.User;
using Cookbook.Exception.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cookbook.Api.Extensions;

public static class AuthenticationExtension
{
    public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var signingKey = configuration.GetValue<string>("Jwt:SigningKey")!;

                options.TokenValidationParameters = new()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var subject = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub) ??
                            context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

                        if (Guid.TryParse(subject, out var userId) == false)
                        {
                            context.Fail("Invalid token subject");

                            return;
                        }

                        var userRepository = context
                            .HttpContext
                            .RequestServices
                            .GetRequiredService<IUserReadOnlyRepository>();

                        var userExists = await userRepository.ExistActiveUserWithIdAsync(userId);

                        if (userExists == false)
                            context.Fail("User not found or inactive");
                    },
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var response = context.AuthenticateFailure switch
                        {
                            null => new ResponseErrorJson(ResourceMessagesException.ACCESS_TOKEN_REQUIRED),
                            SecurityTokenExpiredException => new ResponseErrorJson(ResourceMessagesException.TOKEN_EXPIRED, accessTokenExpired: true),
                            _ => new ResponseErrorJson(ResourceMessagesException.RESOURCE_ACCESS_DENIED)
                        };

                        await context.Response.WriteAsJsonAsync(response);
                    }
                };
            });

        return services;
    }
}
