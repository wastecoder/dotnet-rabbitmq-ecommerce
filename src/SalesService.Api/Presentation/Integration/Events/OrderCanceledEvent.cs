namespace SalesService.Api.Presentation.Integration.Events;

public record OrderCanceledEvent(
    Guid OrderId,
    DateTime CanceledAt,
    string? Reason
);