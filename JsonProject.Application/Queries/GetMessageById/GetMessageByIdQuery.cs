using JsonProject.Domain.Entities;
using JsonProject.Application.Core.Abstractions.Messaging;
using JsonProject.Domain.Common.Core.Primitives.Maybe;

namespace JsonProject.Application.Queries.GetMessageById;

/// <summary>
/// Represents the get message by identifier query record.
/// </summary>
/// <param name="MessageId"></param>
public sealed record GetMessageByIdQuery(Guid MessageId)
    : IQuery<Maybe<Message>>;