using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Catalog.Application.UseCases;
using ModularMonolith.Catalog.Domain.Interfaces;
using ModularMonolith.Catalog.Infraestructure.Data;
using ModularMonolith.Catalog.Infraestructure.Shared;
using ModularMonolith.Catalog.Shared;

namespace ModularMonolith.Catalog.Infraestructure;

public static class CatalogConfigurationModule
{
    public static IServiceCollection AddCatalogConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddAppSettingsConfiguration<IdentityOptions>(configuration, "Identity");

        //UseCases
        services.AddScoped<IGetProdutsUseCase, GetProdutsUseCase>();
        services.AddScoped<IGetProductByIdUseCase, GetProductByIdUseCase>();

        //Repositories
        services.AddSingleton<IProductRepository, ProductRepository>();

        //Shared configuration for Identity module
        services.AddSingleton<IGetProductsSharedUseCase, GetProductsImplementation>();

        return services;
    }
}