using Gateway.Api.Domain.Entities;
using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Presentation.Contracts.Responses;

namespace Gateway.Api.Application.Orchestrators;

public class MetricsOrchestrator(
    IProductMetricsRepository productMetricsRepository,
    IProductSalesStatsRepository salesStatsRepository,
    ISalesMetricsRepository salesCountersRepository
) : IMetricsOrchestrator
{
    public async Task<MetricsDashboardResponse> GetDashboardAsync(int topSellingCount)
    {
        var (productMetrics, salesMetrics) = await LoadAllMetricsAsync();

        var inventory = BuildInventory(productMetrics);
        var sales = BuildSales(salesMetrics);
        var topSelling = await salesStatsRepository.GetTopSellingAsync(topSellingCount);
        var lastUpdated = ComputeLastUpdated(productMetrics, salesMetrics);

        return new MetricsDashboardResponse(
            Inventory: inventory,
            Sales: sales,
            TopSelling: topSelling,
            LastUpdated: lastUpdated
        );
    }


    private async Task<(IReadOnlyList<ProductMetrics>, SalesMetrics?)> LoadAllMetricsAsync()
    {
        var productMetricsTask = productMetricsRepository.GetAllAsync();
        var salesMetricsTask = salesCountersRepository.GetAsync();

        await Task.WhenAll(productMetricsTask, salesMetricsTask);

        return (productMetricsTask.Result, salesMetricsTask.Result);
    }

    private InventoryMetricsDto BuildInventory(IReadOnlyList<ProductMetrics> metrics)
    {
        var totalProducts = metrics.Count;
        var lowStockProducts = metrics.Count(m => m.Quantity <= m.LowStockThreshold);

        return new InventoryMetricsDto(
            TotalProducts: totalProducts,
            LowStockProducts: lowStockProducts
        );
    }

    private SalesMetricsDto BuildSales(SalesMetrics? salesMetrics)
    {
        if (salesMetrics is null)
        {
            return new SalesMetricsDto(
                TotalSales: 0,
                ConfirmedSales: 0,
                CanceledSales: 0
            );
        }

        return new SalesMetricsDto(
            TotalSales: salesMetrics.TotalSales,
            ConfirmedSales: salesMetrics.ConfirmedSales,
            CanceledSales: salesMetrics.CanceledSales
        );
    }

    // It needs future optimization
    private DateTimeOffset ComputeLastUpdated(
        IReadOnlyList<ProductMetrics> productMetrics,
        SalesMetrics? salesMetrics
    )
    {
        var lastInventoryUpdated = productMetrics.Any()
            ? productMetrics.Max(m => m.LastUpdated)
            : DateTimeOffset.MinValue;

        var lastSalesUpdated = salesMetrics?.LastUpdated ?? DateTimeOffset.MinValue;

        return lastInventoryUpdated > lastSalesUpdated
            ? lastInventoryUpdated
            : lastSalesUpdated;
    }
}