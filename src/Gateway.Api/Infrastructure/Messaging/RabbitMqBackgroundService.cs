using Gateway.Api.Infrastructure.Messaging.Abstractions;
using Gateway.Api.Infrastructure.Messaging.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Gateway.Api.Infrastructure.Messaging;

public class RabbitMqBackgroundService<TEvent>(
    RabbitMqConnection connection,
    IServiceScopeFactory scopeFactory,
    string queueName,
    string[] routingKeys
) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        connection.DeclareQueueAndBind(queueName, routingKeys);

        var channel = connection.Channel;

        var asyncConsumer = new AsyncEventingBasicConsumer(channel);

        asyncConsumer.Received += async (_, args) =>
        {
            using var scope = scopeFactory.CreateScope();
            var consumer =
                scope.ServiceProvider.GetRequiredService<IRabbitMqConsumer<TEvent>>();

            try
            {
                var message = connection.Deserialize<TEvent>(args.Body);

                // Executes business logic
                await consumer.HandleAsync(message, stoppingToken);

                // Ack in case of success
                channel.BasicAck(args.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                // Conditioned requeue - TODO: add DLQ
                var shouldRequeue =
                    ex is TimeoutException ||
                    ex is TaskCanceledException ||
                    ex is OperationCanceledException;

                channel.BasicNack(
                    args.DeliveryTag,
                    multiple: false,
                    requeue: shouldRequeue
                );
            }
        };

        channel.BasicConsume(
            queue: queueName,
            autoAck: false,
            consumer: asyncConsumer
        );

        return Task.CompletedTask;
    }
}