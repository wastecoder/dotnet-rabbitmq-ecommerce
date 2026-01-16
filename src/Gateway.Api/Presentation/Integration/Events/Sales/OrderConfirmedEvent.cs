namespace Gateway.Api.Presentation.Integration.Events.Sales;

public record OrderConfirmedEvent(
    Guid OrderId,
    DateTime ConfirmedAt
);