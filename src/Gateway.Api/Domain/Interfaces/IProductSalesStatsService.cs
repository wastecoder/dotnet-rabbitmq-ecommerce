using Gateway.Api.Presentation.Integration.Events.Sales;

namespace Gateway.Api.Domain.Interfaces;

public interface IProductSalesStatsService
{
    Task IncrementProductSalesAsync(OrderCreatedEvent e);
}