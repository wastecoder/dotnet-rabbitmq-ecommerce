using InventoryService.Api.Domain.Entities;

namespace InventoryService.Api.Domain.Interfaces;

public interface IProductService
{
    Task<Product> CreateAsync(string name, decimal price, int quantity, string description);
    Task<Product?> GetByIdAsync(Guid id);
    Task<List<Product>> GetAllAsync();
    Task<Product?> UpdateAsync(Guid id, string name, decimal price, int quantity, string description);
    Task<bool> SoftDeleteAsync(Guid id);
}