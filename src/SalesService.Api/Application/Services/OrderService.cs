using SalesService.Api.Domain.Entities;
using SalesService.Api.Domain.Exceptions;
using SalesService.Api.Domain.Interfaces;
using SalesService.Api.Domain.Messages;
using SalesService.Api.Presentation.Contracts.Requests;

namespace SalesService.Api.Application.Services;

public class OrderService(IOrderRepository repository, IInventoryClient inventoryClient) : IOrderService
{
    public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
    {
        var order = new Order(request.Notes ?? string.Empty);

        // 1. Validate stock for each item
        foreach (var item in request.Items)
        {
            var hasStock = await inventoryClient.CheckStockAsync(
                new OrderItemStockCheckDto(item.ProductId, item.Quantity));

            if (!hasStock)
                throw new BusinessValidationException($"Insufficient stock for product {item.ProductId}.");
        }

        // 2. Build order entity
        foreach (var item in request.Items)
        {
            var product = await inventoryClient.GetProductByIdAsync(item.ProductId);
            var orderItem = new OrderItem(
                product.Id,
                product.Name,
                product.Price,
                item.Quantity
            );

            order.AddItem(orderItem);
        }

        // 3. Persist order
        await repository.AddAsync(order);
        await repository.SaveChangesAsync();

        // 4. Decrease stock for each item
        var successfulDecreases = new List<OrderItem>();

        try
        {
            foreach (var item in order.Items)
            {
                await inventoryClient
                    .DecreaseStockAsync(new OrderItemStockUpdateDto(item.ProductId, item.Quantity));

                successfulDecreases.Add(item);
            }
        }
        catch (Exception)
        {
            // 5. Compensation / rollback stock
            foreach (var item in successfulDecreases)
            {
                await inventoryClient.IncreaseStockAsync(
                    new OrderItemStockUpdateDto(item.ProductId, item.Quantity));
            }
            throw;
        }

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

        // Update notes
        order.UpdateNotes(request.Notes ?? string.Empty);

        // Build a map of existing items for quick lookup
        var existingItemsMap = order.Items.ToDictionary(i => i.ProductId);

        var newItems = new List<OrderItem>();
        var stockAdjustments = new List<(OrderItem item, int diff)>(); // diff > 0 -> decrease, diff < 0 -> increase

        foreach (var itemRequest in request.Items)
        {
            var product = await inventoryClient.GetProductByIdAsync(itemRequest.ProductId);
            if (!existingItemsMap.TryGetValue(itemRequest.ProductId, out var existingItem))
            {
                // New item added to order
                var newItem = new OrderItem(
                    product.Id,
                    product.Name,
                    product.Price,
                    itemRequest.Quantity
                );

                // Check stock only if quantity > 0
                if (itemRequest.Quantity > 0)
                {
                    var hasStock = await inventoryClient.CheckStockAsync(
                        new OrderItemStockCheckDto(product.Id, itemRequest.Quantity));
                    if (!hasStock)
                        throw new BusinessValidationException($"Insufficient stock for product {product.Id}.");
                }

                newItems.Add(newItem);
                stockAdjustments.Add((newItem, itemRequest.Quantity)); // need to decrease stock later
            }
            else
            {
                // Existing item, compute quantity difference
                var diff = itemRequest.Quantity - existingItem.Quantity;

                if (diff > 0)
                {
                    // Validate additional stock only if increasing quantity
                    var hasStock = await inventoryClient.CheckStockAsync(
                        new OrderItemStockCheckDto(product.Id, diff));
                    if (!hasStock)
                        throw new BusinessValidationException($"Insufficient stock for product {product.Id}.");
                }

                // Apply updates in-place
                existingItem.UpdateDetails(product.Name, product.Price, itemRequest.Quantity);

                newItems.Add(existingItem);

                if (diff != 0)
                    stockAdjustments.Add((existingItem, diff));
            }
        }

        // Remove items no longer in the request
        var itemsToRemove = order.Items
            .Where(i => request.Items.All(r => r.ProductId != i.ProductId))
            .ToList();
        foreach (var removed in itemsToRemove)
        {
            order.RemoveItem(removed.Id);
            stockAdjustments.Add((removed, -removed.Quantity)); // return stock to inventory
        }

        // Update order items collection
        order.AddItems(newItems);

        // Persist order and adjust stock
        await repository.UpdateAsync(order);
        await repository.SaveChangesAsync();

        // Apply stock adjustments with compensation in case of failure
        var successfulAdjustments = new List<(OrderItem, int)>();
        try
        {
            foreach (var (item, diff) in stockAdjustments)
            {
                if (diff > 0)
                    await inventoryClient.DecreaseStockAsync(new OrderItemStockUpdateDto(item.ProductId, diff));
                else if (diff < 0)
                    await inventoryClient.IncreaseStockAsync(new OrderItemStockUpdateDto(item.ProductId, -diff));

                successfulAdjustments.Add((item, diff));
            }
        }
        catch (Exception)
        {
            // Rollback stock changes if any adjustment fails
            foreach (var (item, diff) in successfulAdjustments)
            {
                if (diff > 0)
                    await inventoryClient.IncreaseStockAsync(new OrderItemStockUpdateDto(item.ProductId, diff));
                else if (diff < 0)
                    await inventoryClient.DecreaseStockAsync(new OrderItemStockUpdateDto(item.ProductId, -diff));
            }
            throw;
        }

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