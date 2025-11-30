namespace SalesService.Api.Presentation.Integration.Events;

public record OrderCreatedEvent(
    Guid OrderId,
    decimal TotalAmount,
    DateTimeOffset CreatedAt,
    IReadOnlyList<OrderCreatedItem> Items
);