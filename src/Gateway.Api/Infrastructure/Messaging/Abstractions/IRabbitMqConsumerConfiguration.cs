namespace Gateway.Api.Infrastructure.Messaging.Abstractions;

public interface IRabbitMqConsumerConfiguration<TEvent>
{
    string QueueName { get; }
    string[] RoutingKeys { get; }
}