namespace JsonProject.Domain.Core.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"Entity {name} ({key}) not found.") { }
}