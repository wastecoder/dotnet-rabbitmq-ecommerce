namespace SalesService.Api.Presentation.Integration.Events;

public record OrderCreatedItem(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);