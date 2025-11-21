using InventoryService.Api.Domain.Entities;
using InventoryService.Api.Domain.Interfaces;

namespace InventoryService.Api.Application.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<Product> CreateAsync(string name, decimal price, int quantity, string description)
    {
        var product = new Product(name, price, quantity, description);

        await repository.AddAsync(product);
        await repository.SaveChangesAsync();

        return product;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<Product?> UpdateAsync(Guid id, string name, decimal price, int quantity, string description)
    {
        var product = await repository.GetByIdAsync(id);
        if (product is null)
            return null;

        product.Update(name, price, quantity, description);

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