using Gateway.Api.Domain.Entities;
using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Infrastructure.Repositories;

public class ProductSalesStatsRepository(AppDbContext db) : IProductSalesStatsRepository
{
    public async Task AddAsync(ProductSalesStats stats)
    {
        await db.ProductSalesStats.AddAsync(stats);
    }

    public async Task<ProductSalesStats?> GetByProductIdAsync(Guid productId)
    {
        return await db.ProductSalesStats
            .FirstOrDefaultAsync(x => x.ProductId == productId);
    }

    public Task UpdateAsync(ProductSalesStats stats)
    {
        db.ProductSalesStats.Update(stats);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}
