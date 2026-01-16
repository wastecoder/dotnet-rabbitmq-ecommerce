namespace Gateway.Api.Presentation.Contracts.Responses;

public record MetricsDashboardResponse(
    InventoryMetricsDto Inventory,
    SalesMetricsDto Sales,
    IReadOnlyList<TopSellingProductDto> TopSelling,
    DateTimeOffset LastUpdated
);

public record InventoryMetricsDto(
    int TotalProducts,
    int LowStockProducts
);

public record SalesMetricsDto(
    int TotalSales,
    int ConfirmedSales,
    int CanceledSales
);

public record TopSellingProductDto(
    Guid ProductId,
    string Name,
    int Sales
);