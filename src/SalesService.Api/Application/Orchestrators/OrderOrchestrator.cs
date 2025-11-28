using SalesService.Api.Domain.Entities;
using SalesService.Api.Domain.Exceptions;
using SalesService.Api.Domain.Interfaces;
using SalesService.Api.Presentation.Contracts.Requests;
using SalesService.Api.Presentation.Contracts.Responses;

namespace SalesService.Api.Application.Orchestrators;

public class OrderOrchestrator(IStockOrchestrator stock) : IOrderOrchestrator
{
    public async Task ValidateStockAsync(IEnumerable<OrderItemRequest> items)
    {
        foreach (var item in items)
        {
            var ok = await stock.CheckStockAsync(item.ProductId, item.Quantity);

            if (!ok)
                throw new BusinessValidationException($"Insufficient stock for product {item.ProductId}");
        }
    }

    public async Task<Dictionary<Guid, ProductResponse>> FetchProductsAsync(
        IEnumerable<Guid> productIds)
    {
        var result = new Dictionary<Guid, ProductResponse>();
        foreach (var id in productIds.Distinct())
        {
            result[id] = await stock.GetProductByIdAsync(id);
        }
        return result;
    }

    public Task<List<OrderItem>> BuildOrderItemsAsync(
        IEnumerable<OrderItemRequest> items,
        Dictionary<Guid, ProductResponse> products)
    {
        var list = new List<OrderItem>();
        foreach (var i in items)
        {
            var p = products[i.ProductId];
            list.Add(new OrderItem(p.Id, p.Name, p.Price, i.Quantity));
        }
        return Task.FromResult(list);
    }

    public Task ApplyStockDecreaseAsync(IEnumerable<(Guid, int)> changes)
    {
        return Task.WhenAll(changes.Select(c => stock.DecreaseStockAsync(c.Item1, c.Item2)));
    }

    public Task ApplyStockIncreaseAsync(IEnumerable<(Guid, int)> changes)
    {
        return Task.WhenAll(changes.Select(c => stock.IncreaseStockAsync(c.Item1, c.Item2)));
    }

    public async Task ExecuteStockAdjustmentsWithRollbackAsync(
        IEnumerable<(Guid productId, int diff)> adjustments)
    {
        var successful = new List<(Guid id, int diff)>();

        try
        {
            foreach (var (productId, diff) in adjustments)
            {
                if (diff > 0)
                    await stock.DecreaseStockAsync(productId, diff);
                else if (diff < 0)
                    await stock.IncreaseStockAsync(productId, -diff);

                successful.Add((productId, diff));
            }
        }
        catch
        {
            // rollback
            foreach (var (productId, diff) in successful)
            {
                if (diff > 0)
                    await stock.IncreaseStockAsync(productId, diff);
                else if (diff < 0)
                    await stock.DecreaseStockAsync(productId, -diff);
            }
            throw;
        }
    }
}