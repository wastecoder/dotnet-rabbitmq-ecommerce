namespace InventoryService.Api.Presentation.Integration.Events;

public record StockLowEvent(
    Guid ProductId,
    string ProductName,
    int Quantity,
    int Threshold,
    DateTime Timestamp
);