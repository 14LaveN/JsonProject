using JsonProject.Application.Commands.CreateMessage;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using JsonProject.Application.Core.Behaviours;

namespace JsonProject.Application;

public static class DiMediator
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddMediatr(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblyContaining<Program>()
                .RegisterServicesFromAssemblies(typeof(CreateMessageCommand).Assembly,
                    typeof(CreateMessageCommandHandler).Assembly);
            
            x.AddOpenBehavior(typeof(BaseTransactionBehavior<,>))
                .AddOpenBehavior(typeof(ValidationBehaviour<,>));
            
            x.Lifetime = ServiceLifetime.Scoped;
        });
        
        return services;
    }
}