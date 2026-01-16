namespace Gateway.Api.Presentation.Integration.Events.Sales;

public record OrderCreatedEvent(
    Guid OrderId,
    decimal TotalAmount,
    DateTimeOffset CreatedAt,
    IReadOnlyList<OrderCreatedItem> Items
);