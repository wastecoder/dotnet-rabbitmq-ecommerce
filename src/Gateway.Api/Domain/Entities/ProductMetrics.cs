namespace Gateway.Api.Domain.Entities;

public class ProductMetrics
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }

    public int Quantity { get; private set; }
    public int LowStockThreshold { get; private set; }

    public DateTimeOffset LastUpdated { get; private set; }


    private ProductMetrics() { }

    public ProductMetrics(Guid productId, string productName, int quantity, int lowStockThreshold)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        LowStockThreshold = lowStockThreshold;
        LastUpdated = DateTimeOffset.UtcNow;
    }


    public void Update(int quantity, int lowStockThreshold)
    {
        Quantity = quantity;
        LowStockThreshold = lowStockThreshold;
        LastUpdated = DateTimeOffset.UtcNow;
    }
}