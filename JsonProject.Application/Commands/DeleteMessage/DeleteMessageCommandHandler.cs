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

namespace JsonProject.Application.Commands.DeleteMessage;

/// <summary>
/// Represents the delete <see cref="Message"/> command handler class.
/// </summary>
/// <param name="logger">The logger.</param>
/// <param name="messagesRepository">The messages repository.</param>
/// <param name="unitOfWork">The unit of work.</param>
internal sealed class DeleteMessageCommandHandler(
    ILogger<DeleteMessageCommandHandler> logger,
    IMessagesRepository messagesRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteMessageCommand, IBaseResponse<Result>>
{
    /// <inehritdoc />
    public async Task<IBaseResponse<Result>> Handle(
        DeleteMessageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Request for delete the message - {request.MessageId} {DateTime.UtcNow}");

            Maybe<Message> maybeMessage = await messagesRepository.GetByIdAsync(request.MessageId);

            if (maybeMessage.HasNoValue)
            {
                logger.LogWarning(DomainErrors.Message.NotFound);
                throw new NotFoundException(DomainErrors.Message.NotFound.Message, DomainErrors.Message.NotFound);
            }
            
            await messagesRepository.Remove(maybeMessage.Value);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            logger.LogInformation($"Message deleted - {DateTime.UtcNow} {maybeMessage.Value.Id}");

            return new BaseResponse<Result>
            {
                Data = await Result.Success(),
                StatusCode = HttpStatusCode.OK,
                Description = "Message deleted"
            };
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"[DeleteMessageCommandHandler]: {exception.Message}");
            return new BaseResponse<Result>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Description = exception.Message
            };
        }
    }
}