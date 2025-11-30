namespace Gateway.Api.Domain.Entities;

public class SalesMetrics
{
    public Guid Id { get; private set; }

    public int TotalSales { get; private set; }
    public int ConfirmedSales { get; private set; }
    public int CanceledSales { get; private set; }

    public DateTimeOffset? LastOrderAt { get; private set; }
    public DateTimeOffset LastUpdated { get; private set; }


    public SalesMetrics()
    {
        Id = Guid.NewGuid();
        LastUpdated = DateTimeOffset.UtcNow;
    }


    public void UpdateCounters(int total, int confirmed, int canceled, DateTimeOffset? lastOrderAt)
    {
        TotalSales = total;
        ConfirmedSales = confirmed;
        CanceledSales = canceled;
        LastOrderAt = lastOrderAt;
        LastUpdated = DateTimeOffset.UtcNow;
    }
}