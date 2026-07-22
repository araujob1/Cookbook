using Cookbook.Application.UseCases.User.Register;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Application.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();

        return services;
    }
}
