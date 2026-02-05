namespace SalesService.Api.Presentation.Integration.Events;

public record OrderConfirmedEvent(
    Guid OrderId,
    DateTimeOffset ConfirmedAt
);