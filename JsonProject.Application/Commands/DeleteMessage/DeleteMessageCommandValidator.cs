using FluentValidation;
using JsonProject.Application.Core.Errors;
using JsonProject.Application.Core.Extensions;

namespace JsonProject.Application.Commands.DeleteMessage;

/// <summary>
/// Represents the <see cref="DeleteMessageCommand"/> validator class.
/// </summary>
public sealed class DeleteMessageCommandValidator
    : AbstractValidator<DeleteMessageCommand>
{
    /// <summary>
    /// Validate the <see cref="DeleteMessageCommand"/>.
    /// </summary>
    public DeleteMessageCommandValidator()
    {
        RuleFor(p =>
                p.MessageId)
            .NotEqual(Guid.Empty)
            .WithError(ValidationErrors.Identifier.IdIsRequired);
    }
}