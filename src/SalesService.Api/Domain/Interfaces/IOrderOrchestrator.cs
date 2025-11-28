using SalesService.Api.Domain.Entities;
using SalesService.Api.Presentation.Contracts.Requests;
using SalesService.Api.Presentation.Contracts.Responses;

namespace SalesService.Api.Domain.Interfaces;

public interface IOrderOrchestrator
{
    Task ValidateStockAsync(
        IEnumerable<OrderItemRequest> items);
    Task<Dictionary<Guid, ProductResponse>> FetchProductsAsync(
        IEnumerable<Guid> productIds);

    Task<List<OrderItem>> BuildOrderItemsAsync(
        IEnumerable<OrderItemRequest> items,
        Dictionary<Guid, ProductResponse> products);

    Task ApplyStockDecreaseAsync(
        IEnumerable<(Guid productId, int qty)> changes);
    Task ApplyStockIncreaseAsync(
        IEnumerable<(Guid productId, int qty)> changes);

    Task ExecuteStockAdjustmentsWithRollbackAsync(
        IEnumerable<(Guid productId, int diff)> adjustments);
}