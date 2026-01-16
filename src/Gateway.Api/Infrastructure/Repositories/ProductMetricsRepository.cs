using Gateway.Api.Domain.Entities;
using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Infrastructure.Repositories;

public class ProductMetricsRepository(AppDbContext db) : IProductMetricsRepository
{
    public async Task AddAsync(ProductMetrics metrics)
    {
        await db.ProductMetrics.AddAsync(metrics);
    }

    public async Task<ProductMetrics?> GetByProductIdAsync(Guid productId)
    {
        return await db.ProductMetrics
            .FirstOrDefaultAsync(x => x.ProductId == productId);
    }

    public async Task<IReadOnlyList<ProductMetrics>> GetAllAsync()
    {
        return await db.ProductMetrics
            .AsNoTracking()
            .ToListAsync();
    }

    public Task UpdateAsync(ProductMetrics metrics)
    {
        db.ProductMetrics.Update(metrics);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}