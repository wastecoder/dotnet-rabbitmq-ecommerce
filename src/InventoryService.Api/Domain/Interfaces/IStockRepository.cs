using InventoryService.Api.Domain.Entities;

namespace InventoryService.Api.Domain.Interfaces;

public interface IStockRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task UpdateAsync(Product product);
    Task SaveChangesAsync();
}