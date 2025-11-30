using InventoryService.Api.Application.Validation;
using InventoryService.Api.Domain.Entities;
using InventoryService.Api.Domain.Exceptions;
using InventoryService.Api.Domain.Interfaces;
using InventoryService.Api.Presentation.Contracts.Requests;

namespace InventoryService.Api.Application.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<Product> CreateAsync(ProductRequest request)
    {
        var validator = new ProductRequestValidator();
        var result = await validator.ValidateAsync(request);

        if (!result.IsValid)
            throw new FluentValidation.ValidationException(result.Errors);

        var product = new Product(
            request.Name,
            request.Price,
            request.Quantity,
            request.Description ?? string.Empty
        );

        await repository.AddAsync(product);
        await repository.SaveChangesAsync();

        return product;
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        var product = await repository.GetByIdAsync(id);

        if (product is null)
            throw new NotFoundException($"Product with ID {id} was not found.");

        return product;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<Product> UpdateAsync(Guid id, ProductRequest request)
    {
        var validator = new ProductRequestValidator();
        var result = await validator.ValidateAsync(request);

        if (!result.IsValid)
            throw new FluentValidation.ValidationException(result.Errors);

        var product = await repository.GetByIdAsync(id);

        if (product is null)
            throw new NotFoundException($"Product with ID {id} was not found.");

        product.Update(request);

        await repository.UpdateAsync(product);
        await repository.SaveChangesAsync();

        return product;
    }

    public async Task SoftDeleteAsync(Guid id)
    {
        var product = await repository.GetByIdAsync(id);

        if (product is null)
            throw new NotFoundException($"Product with ID {id} was not found.");

        await repository.DeleteAsync(product);
        await repository.SaveChangesAsync();
    }
}