using Gateway.Api.Infrastructure.Messaging.Abstractions;
using Gateway.Api.Infrastructure.Messaging.RabbitMq;
using Gateway.Api.Presentation.Integration.Events.Inventory;

namespace Gateway.Api.Infrastructure.Messaging.Consumers;

public class StockUpdatedHostedService
    : RabbitMqBackgroundService<StockUpdatedEvent>
{
    public StockUpdatedHostedService(
        RabbitMqConnection connection,
        IServiceScopeFactory scopeFactory,
        IRabbitMqConsumerConfiguration<StockUpdatedEvent> config)
        : base(connection, scopeFactory, config)
    {
    }
}