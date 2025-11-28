using SalesService.Api.Domain.Interfaces;
using SalesService.Api.Domain.Messages;
using SalesService.Api.Presentation.Contracts.Responses;

namespace SalesService.Api.Application.Orchestrators;

public class StockOrchestrator(IInventoryClient inventory) : IStockOrchestrator
{
    public Task<bool> CheckStockAsync(Guid productId, int qty)
    {
        return inventory.CheckStockAsync(new OrderItemStockCheckDto(productId, qty));
    }

    public Task<ProductResponse> GetProductByIdAsync(Guid productId)
    {
        return inventory.GetProductByIdAsync(productId);
    }

    public Task DecreaseStockAsync(Guid productId, int quantity)
    {
        return inventory.DecreaseStockAsync(new OrderItemStockUpdateDto(productId, quantity));
    }

    public Task IncreaseStockAsync(Guid productId, int quantity)
    {
        return inventory.IncreaseStockAsync(new OrderItemStockUpdateDto(productId, quantity));
    }
}