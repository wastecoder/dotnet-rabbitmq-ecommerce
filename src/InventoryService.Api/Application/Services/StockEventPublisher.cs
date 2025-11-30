using InventoryService.Api.Domain.Entities;
using InventoryService.Api.Domain.Interfaces;
using InventoryService.Api.Presentation.Integration.Events;

namespace InventoryService.Api.Application.Services;

public class StockEventPublisher(IRabbitMqProducer producer)
{
    private const int Threshold = 5;

    public async Task PublishStockUpdatedAsync(Product product)
    {
        var updatedEvent = new StockUpdatedEvent(
            product.Id,
            product.Name,
            product.Quantity,
            DateTimeOffset.UtcNow
        );

        await producer.PublishAsync("stock.updated", updatedEvent);

        if (product.Quantity <= Threshold)
        {
            var lowEvent = new StockLowEvent(
                product.Id,
                product.Name,
                product.Quantity,
                Threshold,
                DateTimeOffset.UtcNow
            );

            await producer.PublishAsync("stock.low", lowEvent);
        }
    }
}