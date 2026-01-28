using System.Text;
using System.Text.Json;
using Gateway.Api.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Gateway.Api.Infrastructure.Messaging.RabbitMq;

public class RabbitMqConnection : IDisposable
{
    private readonly RabbitMqSettings _settings;
    private readonly IConnection _connection;

    public RabbitMqConnection(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value;

        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.Username,
            Password = _settings.Password,
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
    }

    public IModel CreateChannel()
    {
        var channel = _connection.CreateModel();

        channel.ExchangeDeclare(
            exchange: _settings.ExchangeName,
            type: ExchangeType.Topic,
            durable: _settings.Durable
        );

        return channel;
    }

    public void DeclareQueueAndBind(
        IModel channel,
        string queueName,
        params string[] routingKeys)
    {
        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        foreach (var routingKey in routingKeys)
        {
            channel.QueueBind(
                queue: queueName,
                exchange: _settings.ExchangeName,
                routingKey: routingKey
            );
        }
    }

    public T Deserialize<T>(ReadOnlyMemory<byte> body)
    {
        var json = Encoding.UTF8.GetString(body.ToArray());
        return JsonSerializer.Deserialize<T>(json)!;
    }

    public void Dispose()
    {
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}