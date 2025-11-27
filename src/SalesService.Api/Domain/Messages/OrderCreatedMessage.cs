namespace SalesService.Api.Domain.Messages;

public record OrderCreatedMessage(
    Guid OrderId,
    decimal TotalAmount,
    IEnumerable<OrderItemMessage> Items
);