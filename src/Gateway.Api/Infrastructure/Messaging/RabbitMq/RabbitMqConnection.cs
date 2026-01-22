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
    private readonly IModel _channel;

    public IModel Channel => _channel;

    public RabbitMqConnection(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value;

        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.Username,
            Password = _settings.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: _settings.ExchangeName,
            type: ExchangeType.Topic,
            durable: _settings.Durable
        );
    }

    public void DeclareQueueAndBind(string queueName, params string[] routingKeys)
    {
        _channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        foreach (var routingKey in routingKeys)
        {
            _channel.QueueBind(
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
        _channel.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}