using System.Net;
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

namespace JsonProject.Application.Commands.UpdateMessage;

/// <summary>
/// Represents the update <see cref="Message"/> command handler class.
/// </summary>
/// <param name="logger">The logger.</param>
/// <param name="messagesRepository">The messages repository.</param>
/// <param name="unitOfWork">The unit of work.</param>
internal sealed class UpdateMessageCommandHandler(
    ILogger<UpdateMessageCommandHandler> logger,
    IMessagesRepository messagesRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateMessageCommand, IBaseResponse<Result>>
{
    /// <inehritdoc />
    public async Task<IBaseResponse<Result>> Handle(
        UpdateMessageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Request for update the message - {request.Description} {DateTime.UtcNow}");

            Maybe<Message> maybeMessage = await messagesRepository.GetByIdAsync(request.MessageId);

            if (maybeMessage.HasNoValue)
            {
                logger.LogWarning(DomainErrors.Message.NotFound);
                throw new NotFoundException(DomainErrors.Message.NotFound.Message, DomainErrors.Message.NotFound);
            }
            
            Result<Message> message = maybeMessage.Value.ChangeDescription(request.Description);

            if (message.IsFailure)
            {
               logger.LogWarning(DomainErrors.Message.NotUpdated);
               return new BaseResponse<Result>
               {
                   Description = DomainErrors.Message.NotUpdated,
                   StatusCode = HttpStatusCode.BadRequest
               };
            }
            
            await messagesRepository.UpdateMessage(message.Value);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            logger.LogInformation($"Message updated - {DateTime.UtcNow} {message.Value.Id}");

            return new BaseResponse<Result>
            {
                Data = await Result.Success(),
                StatusCode = HttpStatusCode.OK,
                Description = "Message updated"
            };
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"[UpdateMessageCommandHandler]: {exception.Message}");
            return new BaseResponse<Result>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Description = exception.Message
            };
        }
    }
}