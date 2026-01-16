using Gateway.Api.Presentation.Integration.Events.Sales;

namespace Gateway.Api.Domain.Interfaces;

public interface ISalesCountersService
{
    Task HandleOrderConfirmedAsync(OrderConfirmedEvent e);
    // Task HandleOrderCanceledAsync(OrderCanceledEvent e);
}