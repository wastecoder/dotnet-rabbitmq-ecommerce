using SalesService.Api.Presentation.Contracts.Responses;

namespace SalesService.Api.Domain.Interfaces;

public interface IStockOrchestrator
{
    Task<bool> CheckStockAsync(Guid productId, int qty);
    Task<ProductResponse> GetProductByIdAsync(Guid productId);
    Task DecreaseStockAsync(Guid productId, int qty);
    Task IncreaseStockAsync(Guid productId, int qty);
}