namespace JsonProject.Application.ApiHelpers.Contracts;

/// <summary>
/// Contains the API endpoint routes.
/// </summary>
public static class ApiRoutes
{
    /// <summary>
    /// Contains the message routes.
    /// </summary>
    public static class Message
    {
        public const string GetById = "message/{messageId:guid}";
            
        public const string Create = "message";

        public const string Update =  "message/{messageId:guid}";

        public const string Delete = "message/{messageId:guid}";
    }
}