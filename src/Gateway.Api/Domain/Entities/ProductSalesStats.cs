namespace Gateway.Api.Domain.Entities;

public class ProductSalesStats
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }

    public int TotalSales { get; private set; }
    public DateTimeOffset LastUpdated { get; private set; }


    private ProductSalesStats() { }

    public ProductSalesStats(Guid productId, string productName, int totalSales)
    {
        ProductId = productId;
        ProductName = productName;
        TotalSales = totalSales;
        LastUpdated = DateTimeOffset.UtcNow;
    }


    public void IncreaseSales(int amount)
    {
        TotalSales += amount;
        LastUpdated = DateTimeOffset.UtcNow;
    }
}