namespace Gateway.Api.Infrastructure.Messaging.Abstractions;

public interface IRabbitMqConsumer<in T>
{
    Task HandleAsync(T message, CancellationToken cancellationToken);
}