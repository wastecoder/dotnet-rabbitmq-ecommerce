namespace Gateway.Api.Presentation.Integration.Events.Sales;

public record OrderCreatedItem(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);