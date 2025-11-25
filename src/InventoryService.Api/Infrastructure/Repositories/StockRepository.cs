using InventoryService.Api.Domain.Entities;
using InventoryService.Api.Domain.Interfaces;
using InventoryService.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Api.Infrastructure.Repositories;

public class StockRepository(InventoryDbContext context) : IStockRepository
{
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await context.Products
            .Where(p => !p.IsDeleted && p.Id == id)
            .FirstOrDefaultAsync();
    }

    public Task UpdateAsync(Product product)
    {
        context.Products.Update(product);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}