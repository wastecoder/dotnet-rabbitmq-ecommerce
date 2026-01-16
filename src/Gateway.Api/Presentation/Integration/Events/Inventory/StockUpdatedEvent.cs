namespace Gateway.Api.Presentation.Integration.Events.Inventory;

public record StockUpdatedEvent(
    Guid ProductId,
    string ProductName,
    int Quantity,
    int LowStockThreshold,
    DateTimeOffset Timestamp
);