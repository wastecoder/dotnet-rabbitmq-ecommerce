using FluentValidation;
using Gateway.Api.Presentation.Contracts.Requests;

namespace Gateway.Api.Application.Validation;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(50).WithMessage("Email can have a maximum of 50 characters.")
            .Must(e => !string.IsNullOrWhiteSpace(e));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(50).WithMessage("Password must be a maximum of 50 characters.");
    }
}