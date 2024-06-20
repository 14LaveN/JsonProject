using System.Net;
using JsonProject.Domain.Core.Errors;
using JsonProject.Domain.Entities;
using JsonProject.Domain.Repository;
using Microsoft.Extensions.Logging;
using JsonProject.Application.ApiHelpers.Responses;
using JsonProject.Application.Core.Abstractions;
using JsonProject.Application.Core.Abstractions.Messaging;
using JsonProject.Domain.Common.Core.Primitives.Result;
using JsonProject.Domain.Core.Primitives.Result;

namespace JsonProject.Application.Commands.CreateMessage;

/// <summary>
/// Represents the create <see cref="Message"/> command handler class.
/// </summary>
/// <param name="logger">The logger.</param>
/// <param name="messagesRepository">The messages repository.</param>
/// <param name="unitOfWork">The unit of work.</param>
internal sealed class CreateMessageCommandHandler(
    ILogger<CreateMessageCommandHandler> logger,
    IMessagesRepository messagesRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateMessageCommand, IBaseResponse<Result>>
{
    /// <inehritdoc />
    public async Task<IBaseResponse<Result>> Handle(
        CreateMessageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Request for create the message - {request.Description} {DateTime.UtcNow}");

            Result<Message> message = Message.Create(request.Description);

            if (message.IsFailure)
            {
               logger.LogWarning(DomainErrors.Message.NotCreated);
               return new BaseResponse<Result>
               {
                   Description = DomainErrors.Message.NotCreated,
                   StatusCode = HttpStatusCode.BadRequest
               };
            }
            
            await messagesRepository.Insert(message.Value);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            logger.LogInformation($"Message created - {message.Value.CreatedOnUtc} {message.Value.Id}");

            return new BaseResponse<Result>
            {
                Data = await Result.Success(),
                StatusCode = HttpStatusCode.OK,
                Description = "Message created"
            };
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"[CreateMessageCommandHandler]: {exception.Message}");
            return new BaseResponse<Result>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Description = exception.Message
            };
        }
    }
}