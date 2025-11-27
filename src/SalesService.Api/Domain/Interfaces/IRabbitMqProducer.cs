using SalesService.Api.Domain.Messages;

namespace SalesService.Api.Domain.Interfaces;

public interface IRabbitMqProducer
{
    Task PublishOrderCreatedAsync(OrderCreatedMessage message);
}