using System.Reflection;
using JsonProject.Application.ApiHelpers.Configurations;
using Microsoft.AspNetCore.Mvc;
using JsonProject.Domain.Core.Utility;

namespace JsonProject.Common.DependencyInjection;

public static class DiSwagger
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddSwagger(
        this IServiceCollection services)
    {
        Ensure.NotNull(services, "Services is required.", nameof(services));

        services.AddSwachbackleService(
            Assembly.GetExecutingAssembly(),
            Assembly.GetExecutingAssembly().GetName().Name!);
        
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });
        
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        
        return services;
    }
}