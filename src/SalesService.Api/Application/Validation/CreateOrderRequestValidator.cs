using FluentValidation;
using SalesService.Api.Presentation.Contracts.Requests;

namespace SalesService.Api.Application.Validation;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes can have a maximum of 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one order item is required.");

        RuleForEach(x => x.Items)
            .SetValidator(new OrderItemRequestValidator());
    }
}