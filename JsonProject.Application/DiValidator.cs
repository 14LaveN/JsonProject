using FluentValidation;
using JsonProject.Application.Commands.CreateMessage;
using JsonProject.Application.Commands.DeleteMessage;
using JsonProject.Application.Commands.UpdateMessage;
using Microsoft.Extensions.DependencyInjection;
using JsonProject.Domain.Core.Utility;

namespace JsonProject.Posts.Application;

public static class DiValidator
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        Ensure.NotNull(services, "Services is required.", nameof(services));

        services
            .AddScoped<IValidator<CreateMessageCommand>, CreateMessageCommandValidator>()
            .AddScoped<IValidator<DeleteMessageCommand>, DeleteMessageCommandValidator>()
            .AddScoped<IValidator<UpdateMessageCommand>, UpdateMessageCommandValidator>();
        
        return services;
    }
}