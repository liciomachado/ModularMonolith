using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Purchase.Application.UseCases;

namespace ModularMonolith.Purchase.Infraestructure;

public static class PuchaseConfigurationModule
{
    public static IServiceCollection AddPurchaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddAppSettingsConfiguration<IdentityOptions>(configuration, "Identity");

        services.AddScoped<IAddItemCartUseCase, AddItemCartUseCase>();

        return services;
    }
}