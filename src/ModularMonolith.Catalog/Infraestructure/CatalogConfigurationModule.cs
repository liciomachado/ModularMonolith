using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularMonolith.Catalog.Application.Services;
using ModularMonolith.Catalog.Application.UseCases;
using ModularMonolith.Catalog.Domain.Interfaces;
using ModularMonolith.Catalog.Infraestructure.Data;
using ModularMonolith.Catalog.Infraestructure.Shared;
using ModularMonolith.Catalog.Shared;
using ModularMonolith.Core.WebApi;
using ModularMonolith.Core.WebApi.Options;

namespace ModularMonolith.Catalog.Infraestructure;

public static class CatalogConfigurationModule
{
    public static IServiceCollection AddCatalogConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppSettingsConfiguration<CatalogOptions>(configuration, "Catalog");

        //UseCases
        services.AddScoped<IEmbeddingService, EmbeddingService>();
        services.AddScoped<IGetProdutsUseCase, GetProdutsUseCase>();
        services.AddScoped<IGetProductByIdUseCase, GetProductByIdUseCase>();
        services.AddScoped<ICreateProductUseCase, CreateProductUseCase>();
        services.AddScoped<ICreateProductsBatchUseCase, CreateProductsBatchUseCase>();
        services.AddScoped<ISyncAllProductsToVectorDbUseCase, SyncAllProductsToVectorDbUseCase>();
        services.AddScoped<IGetSimilarProductsUseCase, GetSimilarProductsUseCase>();

        //Repositories
        services.AddSingleton<IProductRepository, ProductRepository>();
        services.AddHttpClient<IVectorDatabaseRepository, VectorDatabaseRepository>((sp, options) =>
        {
            var catalogOptions = sp.GetRequiredService<IOptions<CatalogOptions>>().Value;
            options.BaseAddress = new Uri(catalogOptions.QdrantApi);
        });

        //Shared configuration for Identity module
        services.AddSingleton<IGetProductsSharedUseCase, GetProductsImplementation>();

        return services;
    }
}