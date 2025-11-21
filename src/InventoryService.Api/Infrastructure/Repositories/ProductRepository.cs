using InventoryService.Api.Domain.Entities;
using InventoryService.Api.Domain.Interfaces;
using InventoryService.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Api.Infrastructure.Repositories;

public class ProductRepository(InventoryDbContext context) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await context.Products
            .Where(p => !p.IsDeleted && p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await context.Products
            .Where(p => !p.IsDeleted)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await context.Products.AddAsync(product);
    }

    public Task UpdateAsync(Product product)
    {
        context.Products.Update(product);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Product product)
    {
        product.IsDeleted = true;
        product.UpdatedAt = DateTimeOffset.UtcNow;
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}