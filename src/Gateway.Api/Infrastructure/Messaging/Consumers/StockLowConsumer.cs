using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Infrastructure.Messaging.Abstractions;
using Gateway.Api.Presentation.Integration.Events.Inventory;

namespace Gateway.Api.Infrastructure.Messaging.Consumers;

public class StockLowConsumer(
    IProductMetricsService productMetricsService
) : IRabbitMqConsumer<StockLowEvent>
{
    public async Task HandleAsync(
        StockLowEvent message,
        CancellationToken cancellationToken)
    {
        await productMetricsService.RegisterLowStockAsync(message);
    }
}