using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Infrastructure.Messaging.Abstractions;
using Gateway.Api.Presentation.Integration.Events.Sales;

namespace Gateway.Api.Infrastructure.Messaging.Consumers;

public class OrderConfirmedConsumer(
    ISalesCountersService countersService
) : IRabbitMqConsumer<OrderConfirmedEvent>
{
    public async Task HandleAsync(
        OrderConfirmedEvent message,
        CancellationToken cancellationToken)
    {
        await countersService.HandleOrderConfirmedAsync(message);
    }
}