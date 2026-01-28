using Gateway.Api.Infrastructure.Messaging.Abstractions;
using Gateway.Api.Infrastructure.Messaging.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Gateway.Api.Infrastructure.Messaging.Hosting;

public class RabbitMqBackgroundService<TEvent>(
    RabbitMqConnection connection,
    IServiceScopeFactory scopeFactory,
    string queueName,
    string[] routingKeys
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = connection.CreateChannel();

        connection.DeclareQueueAndBind(
            channel,
            queueName,
            routingKeys);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (_, args) =>
        {
            using var scope = scopeFactory.CreateScope();

            var handler =
                scope.ServiceProvider.GetRequiredService<IRabbitMqConsumer<TEvent>>();

            try
            {
                var message = connection.Deserialize<TEvent>(args.Body);

                await handler.HandleAsync(message, stoppingToken);

                channel.BasicAck(args.DeliveryTag, false);
            }
            catch
            {
                channel.BasicNack(args.DeliveryTag, false, false);
            }
        };

        channel.BasicConsume(
            queue: queueName,
            autoAck: false,
            consumer: consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}