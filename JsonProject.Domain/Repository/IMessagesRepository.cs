using JsonProject.Domain.Entities;
using JsonProject.Domain.Common.Core.Primitives.Maybe;
using JsonProject.Domain.Common.Core.Primitives.Result;
using JsonProject.Domain.Core.Primitives.Result;

namespace JsonProject.Domain.Repository;

/// <summary>
/// Represents the messages repository interface.
/// </summary>
public interface IMessagesRepository
{
    /// <summary>
    /// Gets the message with the specified identifier.
    /// </summary>
    /// <param name="messageId">The message identifier.</param>
    /// <returns>The maybe instance that may contain the message with the specified identifier.</returns>
    Task<Maybe<Message>> GetByIdAsync(Guid messageId);
    
    /// <summary>
    /// Inserts the specified message to the database.
    /// </summary>
    /// <param name="message">The message to be inserted to the database.</param>
    Task<Result> Insert(Message message);

    /// <summary>
    /// Remove the specified message entity to the database.
    /// </summary>
    /// <param name="message">The message to be inserted to the database.</param>
    Task Remove(Message message);
    
    /// <summary>
    /// Update the specified message entity to the database.
    /// </summary>
    /// <param name="message">The message to be inserted to the database.</param>
    /// <returns>The result instance that may contain the message entity with the specified message class.</returns>
    Task<Result> UpdateMessage(Message? message);
}