using InventoryService.Api.Domain.Entities;
using InventoryService.Api.Presentation.Contracts.Requests;

namespace InventoryService.Api.Domain.Interfaces;

public interface IProductService
{
    Task<Product> CreateAsync(ProductRequest request);
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> UpdateAsync(Guid id, ProductRequest request);
    Task<bool> SoftDeleteAsync(Guid id);
}