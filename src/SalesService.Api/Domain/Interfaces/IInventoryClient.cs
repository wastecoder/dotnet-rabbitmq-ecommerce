using SalesService.Api.Domain.Messages;
using SalesService.Api.Presentation.Contracts.Responses;

namespace SalesService.Api.Domain.Interfaces;

public interface IInventoryClient
{
    Task<bool> CheckStockAsync(OrderItemStockCheckDto item);
    Task<StockUpdatedResponse> IncreaseStockAsync(OrderItemStockUpdateDto item);
    Task<StockUpdatedResponse> DecreaseStockAsync(OrderItemStockUpdateDto item);
    Task<ProductResponse> GetProductByIdAsync(Guid id);
}