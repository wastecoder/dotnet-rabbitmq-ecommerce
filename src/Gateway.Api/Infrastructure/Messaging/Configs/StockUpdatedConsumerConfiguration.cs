using Gateway.Api.Infrastructure.Messaging.Abstractions;
using Gateway.Api.Presentation.Integration.Events.Inventory;

namespace Gateway.Api.Infrastructure.Messaging.Configs;

public class StockUpdatedConsumerConfiguration
    : IRabbitMqConsumerConfiguration<StockUpdatedEvent>
{
    public string QueueName => "gateway.metrics.stock";
    public string[] RoutingKeys => ["stock.updated"];
}