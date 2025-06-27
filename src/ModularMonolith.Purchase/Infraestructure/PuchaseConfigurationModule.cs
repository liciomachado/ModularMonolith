using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Purchase.Application.UseCases;
using ModularMonolith.Purchase.Domain.Interfaces;
using ModularMonolith.Purchase.Infraestructure.Data;

namespace ModularMonolith.Purchase.Infraestructure;

public static class PuchaseConfigurationModule
{
    public static IServiceCollection AddPurchaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddAppSettingsConfiguration<IdentityOptions>(configuration, "Identity");

        services.AddScoped<IAddItemCartUseCase, AddItemCartUseCase>();
        services.AddScoped<IRemoveItemCartUseCase, RemoveItemCartUseCase>();
        services.AddScoped<IUpdateItemCartUseCase, UpdateItemCartUseCase>();
        services.AddScoped<IGetCartByUserUseCase, GetCartByUserUseCase>();


        //Repositories
        services.AddSingleton<ICartRepository, CartRepository>();

        return services;
    }
}