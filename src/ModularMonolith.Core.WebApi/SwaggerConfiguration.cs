using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ModularMonolith.Core.WebApi;

public static class SwaggerConfiguration
{
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API Modular Monolith",
                Version = "v1",
                TermsOfService = new Uri("https://agrotools.com.br/"),
                Contact = new OpenApiContact { Email = "dev@agrotools.com.br", Name = "Time de desenvolvimento" },
                Description = "Api modelo de arquitetura modular",
            });

            var authenticationScheme = new OpenApiSecurityScheme
            {
                Description = "Insira somente seu token JWT \r\n\r\n Digite 'Bearer' [space] e em seguida seu token gerado.\r\n\r\nExemplo: Bearer 12345abcdef",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            };

            c.AddSecurityDefinition(authenticationScheme.Reference.Id,
                authenticationScheme);

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        authenticationScheme, new string[] {}
                    }
            });

            var tokenKeyScheme = new OpenApiSecurityScheme
            {
                Description = "Insira o token Key",
                Name = "tokenKey",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Basic",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "TokenKey"
                },
            };
            c.AddSecurityDefinition(tokenKeyScheme.Reference.Id, tokenKeyScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    tokenKeyScheme, new string[] {}
                }
            });

            var apiKeyScheme = new OpenApiSecurityScheme
            {
                Description = "Insira sua api-key",
                Name = "X-Api-Key",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Basic",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "X-Api-Key"
                },
            };
            c.AddSecurityDefinition(apiKeyScheme.Reference.Id, apiKeyScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    apiKeyScheme, new string[] {}
                }
            });
        });
    }

    public static void UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}

