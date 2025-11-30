namespace Gateway.Api.Domain.Entities;

public class StockAlert
{
    public Guid Id { get; private set; }
    public string Type { get; private set; }
    public Guid ProductId { get; private set; }
    public string Message { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }


    private StockAlert() { }

    public StockAlert(string type, Guid productId, string message)
    {
        Id = Guid.NewGuid();
        Type = type;
        ProductId = productId;
        Message = message;
        Timestamp = DateTimeOffset.UtcNow;
    }
}