using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Core.WebApi;

public static class AppSettingsConfiguration
{
    public static IServiceCollection AddAppSettingsConfiguration<TOptions>(
        this IServiceCollection services,
        IConfiguration existingConfiguration,
        string moduleName,
        string? sectionName = null)
        where TOptions : class
    {
        var configFileName = $"appsettings.{moduleName}.json";
        var baseDirectory = AppContext.BaseDirectory;
        var filePath = Path.Combine(baseDirectory, configFileName);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Configuration file '{configFileName}' not found in '{baseDirectory}'");

        // Adiciona o novo JSON ao IConfigurationBuilder existente
        existingConfiguration = new ConfigurationBuilder()
            .AddConfiguration(existingConfiguration) // preserva as configs existentes
            .AddJsonFile(filePath, optional: false, reloadOnChange: true)
            .Build();

        // Bind com validação
        var section = existingConfiguration.GetSection(sectionName ?? typeof(TOptions).Name);
        services.AddOptions<TOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}