namespace InventoryService.Api.Domain.Interfaces;

public interface IRabbitMqProducer
{
    Task PublishAsync<T>(string routingKey, T @event);
}