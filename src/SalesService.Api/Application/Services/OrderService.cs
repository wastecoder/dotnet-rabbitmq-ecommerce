using SalesService.Api.Application.Helpers;
using SalesService.Api.Domain.Entities;
using SalesService.Api.Domain.Exceptions;
using SalesService.Api.Domain.Interfaces;
using SalesService.Api.Presentation.Contracts.Requests;

namespace SalesService.Api.Application.Services;

public class OrderService(IOrderRepository repository, IOrderOrchestrator orderOrchestrator) : IOrderService
{
    public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
    {
        // 1. validate stock
        await orderOrchestrator.ValidateStockAsync(request.Items);

        // 2. fetch products once
        var productIds = request.Items.Select(i => i.ProductId);
        var products = await orderOrchestrator.FetchProductsAsync(productIds);

        // 3. build items
        var orderItems = await orderOrchestrator.BuildOrderItemsAsync(request.Items, products);

        var order = new Order(request.Notes);
        order.AddItems(orderItems);

        // 4. apply stock decrease with rollback only AFTER DB is saved
        var adjustments = orderItems
            .Select(i => (i.ProductId, i.Quantity))
            .ToList();

        // 5. atomic-like operation:
        await repository.AddAsync(order);
        await repository.SaveChangesAsync();

        await orderOrchestrator.ExecuteStockAdjustmentsWithRollbackAsync(adjustments);

        // 6. Publish event (will implement later)
        // await _rabbitMqProducer.PublishOrderCreatedAsync(new OrderCreatedMessage(order));

        return order;
    }

    public async Task<Order> GetOrderByIdAsync(Guid id)
    {
        var order = await repository.GetByIdAsync(id);

        if (order is null)
            throw new NotFoundException($"Order with ID {id} was not found.");

        return order;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<Order> UpdateOrderAsync(Guid id, CreateOrderRequest request)
    {
        var order = await repository.GetByIdAsync(id);
        if (order is null)
            throw new NotFoundException($"Order with ID {id} was not found.");

        // 1. Update order notes
        order.UpdateNotes(request.Notes ?? string.Empty);

        // 2. Fetch only needed products (new items or changed quantities)
        var productIds = request.Items.Select(i => i.ProductId).Distinct();
        var products = await orderOrchestrator.FetchProductsAsync(productIds);

        // 3. Calculate diffs
        var result = OrderUpdateCalculator.Calculate(order.Items, request.Items, products);

        // 4. Validate stock for new or increased items
        var itemsToValidate = result.NewItems
            .Concat(result.UpdatedItems
                .Where(u => result.StockAdjustments.Any(a => a.productId == u.ProductId && a.diff > 0)))
            .Select(i => new OrderItemRequest(i.ProductId, i.Quantity))
            .ToList();

        if (itemsToValidate.Count > 0)
            await orderOrchestrator.ValidateStockAsync(itemsToValidate);

        // 5. Apply changes in Order entity
        foreach (var removed in result.RemovedItems)
            order.RemoveItem(removed.Id);

        foreach (var updated in result.UpdatedItems)
            order.UpdateItem(updated);

        foreach (var added in result.NewItems)
            order.AddItem(added);

        // 6. Persist order
        await repository.UpdateAsync(order);
        await repository.SaveChangesAsync();

        // 7. Apply stock adjustments with rollback
        if (result.StockAdjustments.Count > 0)
            await orderOrchestrator.ExecuteStockAdjustmentsWithRollbackAsync(result.StockAdjustments);

        return order;
    }

    public async Task SoftDeleteOrderAsync(Guid id)
    {
        var order = await repository.GetByIdAsync(id);

        if (order is null)
            throw new NotFoundException($"Order with ID {id} was not found.");

        await repository.DeleteAsync(order);
        await repository.SaveChangesAsync();
    }
}