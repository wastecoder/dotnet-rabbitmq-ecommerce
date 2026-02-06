using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Infrastructure.Messaging.Abstractions;
using Gateway.Api.Presentation.Integration.Events.Sales;

namespace Gateway.Api.Infrastructure.Messaging.Consumers;

public class OrderCreatedConsumer(
    IProductSalesStatsService statsService
) : IRabbitMqConsumer<OrderCreatedEvent>
{
    public async Task HandleAsync(
        OrderCreatedEvent message,
        CancellationToken cancellationToken)
    {
        await statsService.IncrementProductSalesAsync(message);
    }
}