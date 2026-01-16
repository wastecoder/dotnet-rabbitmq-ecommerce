using Gateway.Api.Presentation.Integration.Events.Inventory;

namespace Gateway.Api.Domain.Interfaces;

public interface IProductMetricsService
{
    Task UpdateStockMetricsAsync(StockUpdatedEvent e);
    Task RegisterLowStockAsync(StockLowEvent e);
}