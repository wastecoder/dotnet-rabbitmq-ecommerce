namespace Gateway.Api.Presentation.Integration.Events.Sales;

public record OrderCanceledEvent(
    Guid OrderId,
    DateTimeOffset CanceledAt
);