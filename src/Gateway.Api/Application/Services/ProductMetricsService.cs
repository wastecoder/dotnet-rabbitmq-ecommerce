using Gateway.Api.Domain.Entities;
using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Presentation.Integration.Events.Inventory;

namespace Gateway.Api.Application.Services;

public class ProductMetricsService(
    IProductMetricsRepository metricsRepository,
    IStockAlertRepository alertRepository
) : IProductMetricsService
{
    public async Task UpdateStockMetricsAsync(StockUpdatedEvent e)
    {
        var metrics = await metricsRepository.GetByProductIdAsync(e.ProductId);

        if (metrics is null)
        {
            metrics = new ProductMetrics(
                e.ProductId,
                e.ProductName,
                e.Quantity,
                e.LowStockThreshold
            );

            await metricsRepository.AddAsync(metrics);
        }
        else
        {
            metrics.Update(
                e.Quantity,
                e.LowStockThreshold
            );

            await metricsRepository.UpdateAsync(metrics);
        }

        await metricsRepository.SaveChangesAsync();
    }

    public async Task RegisterLowStockAsync(StockLowEvent e)
    {
        var message =
            $"Low stock alert for product '{e.ProductName}'. " +
            $"Current quantity: {e.Quantity}. Threshold: {e.Threshold}.";

        var alert = new StockAlert(
            type: "LOW_STOCK",
            productId: e.ProductId,
            message: message
        );

        await alertRepository.AddAsync(alert);
        await alertRepository.SaveChangesAsync();
    }
}