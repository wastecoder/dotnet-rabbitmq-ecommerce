using Gateway.Api.Infrastructure.Messaging.Abstractions;
using Gateway.Api.Infrastructure.Messaging.RabbitMq;

namespace Gateway.Api.Infrastructure.Messaging.Hosting;

public static class RabbitMqServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMqConsumer<TEvent, TConsumer>(
        this IServiceCollection services,
        string queue,
        string[] routingKeys)
        where TConsumer : class, IRabbitMqConsumer<TEvent>
    {
        services.AddScoped<IRabbitMqConsumer<TEvent>, TConsumer>();

        services.AddHostedService(sp =>
            new RabbitMqBackgroundService<TEvent>(
                sp.GetRequiredService<RabbitMqConnection>(),
                sp.GetRequiredService<IServiceScopeFactory>(),
                queue,
                routingKeys
            ));

        return services;
    }
}