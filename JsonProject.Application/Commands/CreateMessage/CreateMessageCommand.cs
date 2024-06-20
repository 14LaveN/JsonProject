using JsonProject.Application.ApiHelpers.Responses;
using JsonProject.Application.Core.Abstractions.Messaging;
using JsonProject.Domain.Common.Core.Primitives.Result;
using JsonProject.Domain.Common.ValueObjects;
using JsonProject.Domain.Entities;

namespace JsonProject.Application.Commands.CreateMessage;

/// <summary>
/// Represents the create <see cref="Message"/> command.
/// </summary>
/// <param name="Description">The description.</param>
public sealed record CreateMessageCommand(
    Name Description)
    : ICommand<IBaseResponse<Result>>
{
    
    /// <summary>
    /// Create the new post from <see cref="CreateMessageCommand"/> class.
    /// </summary>
    /// <param name="request">The create message request.</param>
    /// <returns>Returns the new message.</returns>
    public static implicit operator Message(CreateMessageCommand request)
    {
        return new Message(request.Description);
    }
}