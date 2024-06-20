using JsonProject.Application.ApiHelpers.Contracts;
using JsonProject.Application.ApiHelpers.Infrastructure;
using JsonProject.Application.ApiHelpers.Policy;
using JsonProject.Application.Commands.CreateMessage;
using JsonProject.Application.Commands.DeleteMessage;
using JsonProject.Application.Commands.UpdateMessage;
using JsonProject.Application.Queries.GetMessageById;
using JsonProject.Domain.Common.Core.Primitives.Maybe;
using JsonProject.Domain.Common.Core.Primitives.Result;
using JsonProject.Domain.Common.ValueObjects;
using JsonProject.Domain.Core.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace JsonProject.Controllers.V1;

/// <summary>
/// Represents the posts controller class.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/message")]
public sealed class JsonController(ISender sender)
    : ApiController(sender)
{
    #region Commands.

    /// <summary>
    /// Create message.
    /// </summary>
    /// <param name="description">The description.</param>
    /// <returns>Base information about create message method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Internal server error.</response>
    [EnableRateLimiting("fixed")]
    [HttpPost(ApiRoutes.Message.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateMessage([FromBody] string description) =>
        await Result.Create(description, DomainErrors.General.UnProcessableRequest)
            .Map(request => new CreateMessageCommand(
                    Name.Create(request).Value))
            .Bind(command => Task.FromResult(BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data))
            .Match(Json, Unauthorized);

    /// <summary>
    /// Register user.
    /// </summary>
    /// <param name="description">The description.</param>
    /// <param name="messageId">The message identifier.</param>
    /// <returns>Base information about update message method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Internal server error.</response>
    [EnableRateLimiting("fixed")]
    [HttpPatch(ApiRoutes.Message.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateMessage(
        [FromBody] string description,
        [FromRoute] Guid messageId) =>
        await Result.Create(description, DomainErrors.General.UnProcessableRequest)
            .Map(request => new UpdateMessageCommand(
                Name.Create(request).Value,
                messageId))
            .Bind(command => Task.FromResult(BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data))
            .Match(Json, Unauthorized);
    
    /// <summary>
    /// Get Message By Id.
    /// </summary>
    /// <param name="messageId">The message identifier.</param>>
    /// <returns>Base information about get message by identifier method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">BadRequest.</response>
    /// <response code="500">Internal server error</response>
    [EnableRateLimiting("fixed")]
    [HttpGet(ApiRoutes.Message.GetById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessageById([FromRoute] Guid messageId) =>
        await Maybe<GetMessageByIdQuery>
            .From(new GetMessageByIdQuery(messageId))
            .Bind(async query => await BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(query)))
            .Match(Json, NotFound);
    
    /// <summary>
    /// Delete message.
    /// </summary>
    /// <param name="messageId">The message identifier.</param>
    /// <returns>Base information about delete message method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Internal server error.</response>
    [EnableRateLimiting("fixed")]
    [HttpPost(ApiRoutes.Message.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteMessage([FromBody] Guid messageId) =>
        await Result.Success(new DeleteMessageCommand(messageId))
            .Bind(command => Task.FromResult(BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data))
            .Match(Json, Unauthorized);
    
    #endregion
}