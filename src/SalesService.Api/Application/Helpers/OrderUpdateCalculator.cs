using SalesService.Api.Domain.Entities;
using SalesService.Api.Presentation.Contracts.Requests;
using SalesService.Api.Presentation.Contracts.Responses;

namespace SalesService.Api.Application.Helpers;

public class OrderUpdateCalculator
{
    public record UpdateResult(
        List<OrderItem
        > NewItems,
        List<OrderItem> UpdatedItems,
        List<OrderItem> RemovedItems,
        List<(Guid productId, int diff)> StockAdjustments
    );

    public static UpdateResult Calculate(
        IEnumerable<OrderItem> existingItems,
        IEnumerable<OrderItemRequest> requestedItems,
        Dictionary<Guid, ProductResponse> products)
    {
        // Force evaluation to avoid multiple enumeration
        var existingList = existingItems.ToList();
        var requestedList = requestedItems.ToList();

        var existingMap = existingList.ToDictionary(i => i.ProductId);
        var requestedMap = requestedList.ToDictionary(r => r.ProductId);

        var newItems = new List<OrderItem>();
        var updatedItems = new List<OrderItem>();
        var removedItems = new List<OrderItem>();
        var stockAdjustments = new List<(Guid productId, int diff)>();

        // Handle new items and updates
        foreach (var request in requestedList)
        {
            var product = products[request.ProductId];
            if (!existingMap.TryGetValue(request.ProductId, out var existing))
            {
                // New item
                var item = new OrderItem(product.Id, product.Name, product.Price, request.Quantity);
                newItems.Add(item);
                stockAdjustments.Add((product.Id, request.Quantity));
            }
            else
            {
                // Existing item - check diff
                var diff = request.Quantity - existing.Quantity;
                if (diff == 0 && product.Price == existing.UnitPrice && product.Name == existing.ProductName)
                    continue;

                existing.UpdateDetails(product.Name, product.Price, request.Quantity);
                updatedItems.Add(existing);
                if (diff != 0)
                    stockAdjustments.Add((existing.ProductId, diff));
            }
        }

        // Removed items
        foreach (var existing in existingList)
        {
            if (requestedMap.ContainsKey(existing.ProductId))
                continue;

            removedItems.Add(existing);
            stockAdjustments.Add((existing.ProductId, -existing.Quantity));
        }

        return new UpdateResult(newItems, updatedItems, removedItems, stockAdjustments);
    }
}