using JsonProject.Domain.Common.Core.Primitives;

namespace JsonProject.Domain.Core.Errors;

/// <summary>
/// Contains the domain errors.
/// </summary>
public static class DomainErrors
{
    /// <summary>
    /// Contains the string errors
    /// </summary>
    public static class StringErrors
    {
        public static Error IsNull =>
            new("StringErrors.IsNull", "The string is null.");
    }

    public static class Message
    {
        public static Error NotCreated =>
            new("Message.NotCreated", "The message not created.");
        
        public static Error NotUpdated =>
            new("Message.NotUpdated", "The message not updated.");
        
        public static Error NotFound =>
            new("Message.NotFound", "The message not found.");
        
        public static Error CannotChangeDescription =>
            new("Message.CannotChangeDescription", "The description cannot be changed to the specified description.");
    }
    
    /// <summary>
    /// Contains the name errors.
    /// </summary>
    public static class Name
    {
        public static Error NullOrEmpty => new("Name.NullOrEmpty", "The name is required.");

        public static Error LongerThanAllowed => new("Name.LongerThanAllowed", "The name is longer than allowed.");
    }

    /// <summary>
    /// Contains general errors.
    /// </summary>
    public static class General
    {
        public static Error UnProcessableRequest => new(
            "General.UnProcessableRequest",
            "The server could not process the request.");

        public static Error ServerError => new("General.ServerError", "The server encountered an unrecoverable error.");
    }
}