using JsonProject.Application.ApiHelpers.Responses;
using JsonProject.Application.Core.Abstractions.Messaging;
using JsonProject.Domain.Common.Core.Primitives.Result;
using JsonProject.Domain.Common.ValueObjects;

namespace JsonProject.Application.Commands.UpdateMessage;

/// <summary>
/// Represents the update message command record.
/// </summary>
/// <param name="Description">The description.</param>
/// <param name="MessageId">The message identifier.</param>
public sealed record UpdateMessageCommand(
    Name Description,
    Guid MessageId)
    : ICommand<IBaseResponse<Result>>;