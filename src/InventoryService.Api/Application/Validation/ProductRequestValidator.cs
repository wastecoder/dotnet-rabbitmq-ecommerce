using FluentValidation;
using InventoryService.Api.Presentation.Contracts.Requests;

namespace InventoryService.Api.Application.Validation;

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name must have at least 3 characters.")
            .MaximumLength(200).WithMessage("Name can have a maximum of 200 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.")
            .LessThanOrEqualTo(10000).WithMessage("Price cannot exceed 10,000.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be positive.")
            .LessThanOrEqualTo(9999).WithMessage("Quantity cannot exceed 9,999.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description can have a maximum of 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}