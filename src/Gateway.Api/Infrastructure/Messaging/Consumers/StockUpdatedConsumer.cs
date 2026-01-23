using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Infrastructure.Messaging.Abstractions;
using Gateway.Api.Presentation.Integration.Events.Inventory;

namespace Gateway.Api.Infrastructure.Messaging.Consumers;

public class StockUpdatedConsumer(
    IProductMetricsService productMetricsService
) : IRabbitMqConsumer<StockUpdatedEvent>
{
    public async Task HandleAsync(
        StockUpdatedEvent message,
        CancellationToken cancellationToken)
    {
        await productMetricsService.UpdateStockMetricsAsync(message);
    }
}