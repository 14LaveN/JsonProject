using System.Net;
using JsonProject.Application.Commands.UpdateMessage;
using JsonProject.Domain.Core.Errors;
using JsonProject.Domain.Entities;
using JsonProject.Domain.Repository;
using Microsoft.Extensions.Logging;
using JsonProject.Application.ApiHelpers.Responses;
using JsonProject.Application.Core.Abstractions;
using JsonProject.Application.Core.Abstractions.Messaging;
using JsonProject.Domain.Common.Core.Primitives.Maybe;
using JsonProject.Domain.Common.Core.Primitives.Result;
using JsonProject.Domain.Core.Exceptions;
using JsonProject.Domain.Core.Primitives.Result;

namespace JsonProject.Application.Queries.GetMessageById;

/// <summary>
/// Represents the get <see cref="Message"/> by identifier query handler class.
/// </summary>
/// <param name="logger">The logger.</param>
/// <param name="messagesRepository">The messages repository.</param>
/// <param name="unitOfWork">The unit of work.</param>
internal sealed class GetMessageByIdQueryHandler(
    ILogger<GetMessageByIdQueryHandler> logger,
    IMessagesRepository messagesRepository,
    IUnitOfWork unitOfWork)
    : IQueryHandler<GetMessageByIdQuery, Maybe<Message>>
{
    /// <inehritdoc />
    public async Task<Maybe<Message>> Handle(
        GetMessageByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Request for get the message by identifier - {request.MessageId} {DateTime.UtcNow}");

            Maybe<Message> maybeMessage = await messagesRepository.GetByIdAsync(request.MessageId);

            if (maybeMessage.HasNoValue)
            {
                logger.LogWarning(DomainErrors.Message.NotFound);
                throw new NotFoundException(DomainErrors.Message.NotFound.Message, DomainErrors.Message.NotFound);
            }
            
            logger.LogInformation($"Get message by identifier - {DateTime.UtcNow} {maybeMessage.Value.Id}");

            return maybeMessage.Value;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"[GetMessageByIdQueryHandler]: {exception.Message}");
            return Maybe<Message>.None;
        }
    }
}