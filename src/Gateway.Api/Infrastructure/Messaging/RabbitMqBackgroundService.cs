using Gateway.Api.Infrastructure.Messaging.Abstractions;
using Gateway.Api.Infrastructure.Messaging.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Gateway.Api.Infrastructure.Messaging;

public class RabbitMqBackgroundService<TEvent> : BackgroundService
{
    private readonly RabbitMqConnection _connection;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IRabbitMqConsumerConfiguration<TEvent> _config;

    public RabbitMqBackgroundService(
        RabbitMqConnection connection,
        IServiceScopeFactory scopeFactory,
        IRabbitMqConsumerConfiguration<TEvent> config)
    {
        _connection = connection;
        _scopeFactory = scopeFactory;
        _config = config;
        Console.WriteLine(">>> CONSTRUCTOR");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _connection.CreateChannel();

        _connection.DeclareQueueAndBind(
            channel,
            _config.QueueName,
            _config.RoutingKeys);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (_, args) =>
        {
            Console.WriteLine(">>> RabbitMQ message RECEIVED");

            using var scope = _scopeFactory.CreateScope();

            var handler =
                scope.ServiceProvider.GetRequiredService<IRabbitMqConsumer<TEvent>>();

            try
            {
                var message = _connection.Deserialize<TEvent>(args.Body);

                await handler.HandleAsync(message, stoppingToken);

                channel.BasicAck(args.DeliveryTag, false);
            }
            catch
            {
                channel.BasicNack(args.DeliveryTag, false, false);
            }
        };

        channel.BasicConsume(
            queue: _config.QueueName,
            autoAck: false,
            consumer: consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}