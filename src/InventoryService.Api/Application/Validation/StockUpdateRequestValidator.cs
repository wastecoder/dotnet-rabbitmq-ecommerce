using FluentValidation;
using InventoryService.Api.Presentation.Contracts.Requests;

namespace InventoryService.Api.Application.Validation;

public class StockUpdateRequestValidator : AbstractValidator<StockUpdateRequest>
{
    public StockUpdateRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}