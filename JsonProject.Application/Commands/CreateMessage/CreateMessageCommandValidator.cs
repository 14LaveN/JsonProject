using FluentValidation;
using JsonProject.Application.Core.Errors;
using JsonProject.Application.Core.Extensions;

namespace JsonProject.Application.Commands.CreateMessage;

/// <summary>
/// Represents the <see cref="CreateMessageCommand"/> validator class.
/// </summary>
public sealed class CreateMessageCommandValidator
    : AbstractValidator<CreateMessageCommand>
{
    /// <summary>
    /// Validate the <see cref="CreateMessageCommand"/>.
    /// </summary>
    public CreateMessageCommandValidator()
    {
        RuleFor(p =>
                p.Description.Value).NotEqual(string.Empty)
            .WithError(ValidationErrors.CreateMessage.DescriptionIsRequired)
            .MaximumLength(512)
            .WithMessage("Your description too big.");
    }
}