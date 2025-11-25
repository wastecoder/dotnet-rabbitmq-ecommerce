using InventoryService.Api.Domain.Entities;

namespace InventoryService.Api.Domain.Interfaces;

public interface IStockService
{
    Task<Product> GetAvailabilityAsync(Guid id);
    Task<(Product product, int oldQuantity)> DecreaseStockAsync(Guid id, int quantity);
    Task<(Product product, int oldQuantity)> IncreaseStockAsync(Guid id, int quantity);
}