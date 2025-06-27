using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Core.WebApi;

public static class InjectControllersConfiguration
{
    public static IMvcBuilder AddDiscoveredControllers<TBaseController>(this IServiceCollection services)
        where TBaseController : ControllerBase
    {
        var assembliesWithControllers = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic && a.GetExportedTypes().Any(t =>
                t.IsClass &&
                !t.IsAbstract &&
                typeof(TBaseController).IsAssignableFrom(t)))
            .Distinct()
            .ToList();

        var mvcBuilder = services.AddControllers();

        mvcBuilder.ConfigureApplicationPartManager(manager =>
        {
            foreach (var assembly in assembliesWithControllers)
            {
                if (!manager.ApplicationParts.Any(p =>
                        p is AssemblyPart ap && ap.Assembly.FullName == assembly.FullName))
                {
                    manager.ApplicationParts.Add(new AssemblyPart(assembly));
                }
            }
        });

        return mvcBuilder;
    }
}