using JsonProject.Application.Core.Abstractions;
using JsonProject.Domain.Repository;
using JsonProject.Presentation.Infrastructure;
using JsonProject.Presentation.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JsonProject.Application.Core.Abstractions;
using JsonProject.Persistence;

namespace JsonProject.Presentation;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddBaseDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var connectionString = configuration.GetConnectionString(ConnectionString.SettingsKey);

        if (connectionString is not null)
            services.AddHealthChecks()
                .AddNpgSql(connectionString);
        
        services.AddDbContext<BaseDbContext>((sp, o) =>
            o.UseNpgsql(connectionString, act
                    =>
            {
                act.EnableRetryOnFailure(3);
                act.CommandTimeout(30);
                act.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            })
                .LogTo(Console.WriteLine)
                .EnableServiceProviderCaching()
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IMessagesRepository, MessagesRepository>();
        services.AddScoped<IDbContext, BaseDbContext>();
        
        return services;
    }
}