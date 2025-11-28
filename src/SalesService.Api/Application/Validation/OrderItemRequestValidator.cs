using FluentValidation;
using SalesService.Api.Presentation.Contracts.Requests;

namespace SalesService.Api.Application.Validation;

public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
{
    public OrderItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.")
            .Must(id => id != Guid.Empty).WithMessage("ProductId cannot be empty.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(9999).WithMessage("Quantity cannot exceed 9,999.");
    }
}