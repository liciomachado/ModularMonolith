using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Core.WebApi;
using ModularMonolith.Identity.Application.Settings;
using ModularMonolith.Identity.Application.UseCases;

namespace ModularMonolith.Identity.Infraestructure;

public static class IdentityConfigurationModule
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppSettingsConfiguration<IdentityOptions>(configuration, "Identity");

        services.AddScoped<ILoginUseCase, LoginUseCase>();

        return services;
    }
}