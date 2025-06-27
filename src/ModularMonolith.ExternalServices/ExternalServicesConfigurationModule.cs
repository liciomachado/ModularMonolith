using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.ExternalServices.Car;

namespace ModularMonolith.ExternalServices;

public static class ExternalServicesConfigurationModule
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ICarGateway, CarFunction>(client =>
        {
            client.BaseAddress = new Uri(configuration["CarBaseUrl"] ?? "https://default-base-url.com/");
        });
        return services;
    }
}