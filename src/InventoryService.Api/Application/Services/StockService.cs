using InventoryService.Api.Application.Validation;
using InventoryService.Api.Domain.Entities;
using InventoryService.Api.Domain.Exceptions;
using InventoryService.Api.Domain.Interfaces;
using InventoryService.Api.Presentation.Contracts.Requests;

namespace InventoryService.Api.Application.Services;

public class StockService(IStockRepository repository) : IStockService
{
    public async Task<Product> GetAvailabilityAsync(Guid id)
    {
        var product = await repository.GetByIdAsync(id);
        if (product is null)
            throw new NotFoundException($"Product with ID {id} was not found.");

        return product;
    }

    public async Task<(Product product, int oldQuantity)> DecreaseStockAsync(Guid id, int quantity)
    {
        var validator = new StockUpdateRequestValidator();
        var validation = await validator.ValidateAsync(new StockUpdateRequest(quantity));
        if (!validation.IsValid)
            throw new FluentValidation.ValidationException(validation.Errors);

        var product = await repository.GetByIdAsync(id);
        if (product is null)
            throw new NotFoundException($"Product with ID {id} was not found.");

        if (product.Quantity < quantity)
            throw new OutOfStockException($"Requested quantity ({quantity}) exceeds available stock ({product.Quantity}).");

        var oldQuantity = product.Quantity;
        product.Quantity -= quantity;
        product.UpdatedAt = DateTimeOffset.UtcNow;

        await repository.UpdateAsync(product);
        await repository.SaveChangesAsync();

        return (product, oldQuantity);
    }

    public async Task<(Product product, int oldQuantity)> IncreaseStockAsync(Guid id, int quantity)
    {
        var validator = new StockUpdateRequestValidator();
        var validation = await validator.ValidateAsync(new StockUpdateRequest(quantity));
        if (!validation.IsValid)
            throw new FluentValidation.ValidationException(validation.Errors);

        var product = await repository.GetByIdAsync(id);
        if (product is null)
            throw new NotFoundException($"Product with ID {id} was not found.");

        var oldQuantity = product.Quantity;
        product.Quantity += quantity;
        product.UpdatedAt = DateTimeOffset.UtcNow;

        await repository.UpdateAsync(product);
        await repository.SaveChangesAsync();

        return (product, oldQuantity);
    }
}