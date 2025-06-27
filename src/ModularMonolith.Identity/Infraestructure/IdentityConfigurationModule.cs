using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Identity.Application.UseCases;

namespace ModularMonolith.Identity.Infraestructure;

public static class IdentityConfigurationModule
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        return services;
    }
}