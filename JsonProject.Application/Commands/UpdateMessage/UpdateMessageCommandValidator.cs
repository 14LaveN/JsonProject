using FluentValidation;
using JsonProject.Application.Core.Errors;
using JsonProject.Application.Core.Extensions;

namespace JsonProject.Application.Commands.UpdateMessage;

/// <summary>
/// Represents the <see cref="UpdateMessageCommand"/> validator class.
/// </summary>
public sealed class UpdateMessageCommandValidator
    : AbstractValidator<UpdateMessageCommand>
{
    /// <summary>
    /// Validate the <see cref="UpdateMessageCommand"/>.
    /// </summary>
    public UpdateMessageCommandValidator()
    {
        RuleFor(p =>
                p.Description.Value).NotEqual(string.Empty)
            .WithError(ValidationErrors.CreateMessage.DescriptionIsRequired)
            .MaximumLength(512)
            .WithMessage("Your description too big.");
        
        RuleFor(p =>
                p.MessageId)
            .NotEqual(Guid.Empty)
            .WithError(ValidationErrors.Identifier.IdIsRequired);
    }
}