using JsonProject.Domain.Core.Errors;
using JsonProject.Domain.Common.Core.Primitives;
using JsonProject.Domain.Common.Core.Primitives.Result;
using JsonProject.Domain.Common.ValueObjects;
using JsonProject.Domain.Core.Primitives.Result;
using JsonProject.Domain.Core.Utility;

namespace JsonProject.Domain.Entities;

/// <summary>
/// Represents the message class.
/// </summary>
public sealed class Message
    : Entity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Message"/> class.
    /// </summary>
    /// <param name="description">The description.</param>
    public Message(
        Name description)
    {
        Ensure.NotEmpty(description, "The description is required", nameof(description));
        
        Description = description;
    }
    
    /// <summary>
    /// The Entity Framework Core ctor.
    /// </summary>
    public Message(){}

    /// <summary>
    /// Gets or sets description.
    /// </summary>
    public Name Description { get; set; } = null!;

    /// <summary>
    /// Gets created on utc.
    /// </summary>
    public DateTime CreatedOnUtc { get; } = DateTime.UtcNow;

    /// <summary>
    /// Creates a new message with the specified description.
    /// </summary>
    /// <param name="description">The description.</param>
    /// <returns>The newly created message instance.</returns>
    public static Result<Message> Create(Name description)
    {
        Message message = new Message(description);

        return message;
    }

    /// <summary>
    /// Change the message with the specified description.
    /// </summary>
    /// <param name="description">The description.</param>
    /// <returns>The newly created message instance.</returns>
    public Result<Message> ChangeDescription(Name description)
    {
        if (Description == description)
        {
            return Result.Failure<Message>(DomainErrors.Message.CannotChangeDescription);
        }

        Description = description;

        return this;
    }
}