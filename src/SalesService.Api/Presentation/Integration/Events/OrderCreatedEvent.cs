namespace SalesService.Api.Presentation.Integration.Events;

public record OrderCreatedEvent(
    Guid OrderId,
    decimal TotalAmount,
    DateTime CreatedAt,
    IReadOnlyList<OrderCreatedItem> Items
);