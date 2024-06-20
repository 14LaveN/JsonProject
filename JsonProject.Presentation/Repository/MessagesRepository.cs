using JsonProject.Domain.Core.Errors;
using JsonProject.Domain.Entities;
using JsonProject.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using JsonProject.Domain.Common.Core.Primitives.Result;
using JsonProject.Domain.Core.Extensions;

namespace JsonProject.Presentation.Repository;

/// <summary>
/// Represents the implementation of <see cref="IMessagesRepository"/>.
/// </summary>
internal sealed class MessagesRepository(BaseDbContext baseDbContext)
    : GenericRepository<Message>(baseDbContext),
    IMessagesRepository
{
    /// <inheritdoc />
    public async Task<Result> UpdateMessage(Message? message)
    {
        int updateMessageResult = await DbContext
            .Set<Message>()
            .WhereIf(
                message is not null,
                m => m.Id == message!.Id)
            .ExecuteUpdateAsync(x => x.
                SetProperty(p =>
                    p.Description,
                    message!.Description));

        return updateMessageResult is not 0 ?
            await Result.Success() :
            await Result.Failure(DomainErrors.StringErrors.IsNull);
    }
}