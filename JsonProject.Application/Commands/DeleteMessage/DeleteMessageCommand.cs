using JsonProject.Application.ApiHelpers.Responses;
using JsonProject.Application.Core.Abstractions.Messaging;
using JsonProject.Domain.Common.Core.Primitives.Result;

namespace JsonProject.Application.Commands.DeleteMessage;

/// <summary>
/// Represents the delete message command record.
/// </summary>
/// <param name="MessageId">The message identifier.</param>
public sealed record DeleteMessageCommand(Guid MessageId)
    : ICommand<IBaseResponse<Result>>;
