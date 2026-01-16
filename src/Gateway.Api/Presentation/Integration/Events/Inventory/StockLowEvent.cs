namespace Gateway.Api.Presentation.Integration.Events.Inventory;

public record StockLowEvent(
    Guid ProductId,
    string ProductName,
    int Quantity,
    int Threshold,
    DateTimeOffset Timestamp
);