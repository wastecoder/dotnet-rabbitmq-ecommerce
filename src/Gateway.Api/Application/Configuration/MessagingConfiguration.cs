using Gateway.Api.Infrastructure.Messaging.Consumers;
using Gateway.Api.Infrastructure.Messaging.Hosting;
using Gateway.Api.Presentation.Integration.Events.Inventory;
using Gateway.Api.Presentation.Integration.Events.Sales;

namespace Gateway.Api.Application.Configuration;

public static class MessagingConfiguration
{
    public static IServiceCollection AddGatewayMessaging(
        this IServiceCollection services)
    {
        services.AddRabbitMqConsumer<StockUpdatedEvent, StockUpdatedConsumer>(
            "gateway.metrics.stock",
            ["stock.updated"]
        );

        services.AddRabbitMqConsumer<StockLowEvent, StockLowConsumer>(
            "gateway.metrics.stock",
            ["stock.low"]
        );

        services.AddRabbitMqConsumer<OrderCreatedEvent, OrderCreatedConsumer>(
            "gateway.metrics.order",
            ["order.created"]
        );

        return services;
    }
}