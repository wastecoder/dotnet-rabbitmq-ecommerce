using InventoryService.Api.Domain.Entities;
using InventoryService.Api.Domain.Interfaces;
using InventoryService.Api.Presentation.Contracts.Requests;

namespace InventoryService.Api.Application.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<Product> CreateAsync(ProductRequest request)
    {
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

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<Product?> UpdateAsync(Guid id, ProductRequest request)
    {
        var product = await repository.GetByIdAsync(id);
        if (product is null)
            return null;

        product.Update(request);

        await repository.UpdateAsync(product);
        await repository.SaveChangesAsync();

        return product;
    }

    public async Task<bool> SoftDeleteAsync(Guid id)
    {
        var product = await repository.GetByIdAsync(id);
        if (product is null)
            return false;

        await repository.DeleteAsync(product);
        await repository.SaveChangesAsync();

        return true;
    }
}