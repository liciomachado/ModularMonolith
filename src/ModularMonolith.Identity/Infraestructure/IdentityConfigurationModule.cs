using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Core.WebApi;
using ModularMonolith.Core.WebApi.Options;
using ModularMonolith.Identity.Application.Services;
using ModularMonolith.Identity.Application.UseCases;
using ModularMonolith.Identity.Domain.Interfaces;
using ModularMonolith.Identity.Infraestructure.Data;
using ModularMonolith.Identity.Infraestructure.SharedImplementations;
using ModularMonolith.Identity.Shared;

namespace ModularMonolith.Identity.Infraestructure;

public static class IdentityConfigurationModule
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppSettingsConfiguration<IdentityOptions>(configuration, "Identity");

        //UseCases
        services.AddScoped<GenerateJwtTokenService>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IRegisterUseCase, RegisterUseCase>();
        services.AddScoped<IGetUserDataUseCase, GetUserDataUseCase>();

        //Repositories
        services.AddSingleton<IUserRepository, UserRepository>();


        //Shared configuration for Identity module
        services.AddScoped<IIdentityUserData, IdentityUserDataImplementation>();

        return services;
    }
}