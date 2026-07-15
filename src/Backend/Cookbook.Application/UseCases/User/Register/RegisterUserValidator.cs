using Cookbook.Communication.Requests;
using Cookbook.Exception.Resources;
using FluentValidation;

namespace Cookbook.Application.UseCases.User.Register;

public sealed class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.NAME_REQUIRED)
            .MaximumLength(100)
            .WithMessage(ResourceMessagesException.NAME_MAX_LENGTH);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.EMAIL_REQUIRED)
            .MaximumLength(255)
            .WithMessage(ResourceMessagesException.EMAIL_MAX_LENGTH);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.PASSWORD_REQUIRED)
            .MaximumLength(2000)
            .WithMessage(ResourceMessagesException.PASSWORD_MAX_LENGTH);

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });
    }
}
