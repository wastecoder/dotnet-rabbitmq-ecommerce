using Gateway.Api.Domain.Entities;
using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Infrastructure.Repositories;

public class StockAlertRepository(AppDbContext db) : IStockAlertRepository
{
    public async Task AddAsync(StockAlert alert)
    {
        await db.StockAlerts.AddAsync(alert);
    }

    public async Task<List<StockAlert>> GetSinceAsync(DateTime sinceUtc)
    {
        return await db.StockAlerts
            .Where(x => x.Timestamp >= sinceUtc)
            .ToListAsync();
    }

    public async Task<List<StockAlert>> GetByProductIdAsync(Guid productId)
    {
        return await db.StockAlerts
            .Where(x => x.ProductId == productId)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}