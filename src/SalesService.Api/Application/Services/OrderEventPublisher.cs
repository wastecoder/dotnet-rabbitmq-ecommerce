using SalesService.Api.Domain.Interfaces;
using SalesService.Api.Presentation.Integration.Events;
using SalesService.Api.Domain.Entities;

namespace SalesService.Api.Application.Services;

public class OrderEventPublisher(IRabbitMqProducer producer)
{
    public async Task PublishOrderCreatedAsync(Order order)
    {
        var createdEvent = new OrderCreatedEvent(
            order.Id,
            order.TotalAmount,
            order.CreatedAt,
            order.Items.Select(i =>
                new OrderCreatedItem(i.ProductId, i.Quantity)
            ).ToList()
        );

        await producer.PublishAsync("order.created", createdEvent);
    }
}