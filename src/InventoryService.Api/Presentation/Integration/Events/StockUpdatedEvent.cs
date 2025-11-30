namespace InventoryService.Api.Presentation.Integration.Events;

public record StockUpdatedEvent(
    Guid ProductId,
    string ProductName,
    int Quantity,
    DateTimeOffset Timestamp
);