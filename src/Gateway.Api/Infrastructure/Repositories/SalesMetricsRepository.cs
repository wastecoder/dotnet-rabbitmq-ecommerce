using Gateway.Api.Domain.Entities;
using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Infrastructure.Repositories;

public class SalesMetricsRepository(AppDbContext db) : ISalesMetricsRepository
{
    public async Task AddAsync(SalesMetrics metrics)
    {
        await db.SalesMetrics.AddAsync(metrics);
    }

    public async Task<SalesMetrics?> GetAsync()
    {
        return await db.SalesMetrics.FirstOrDefaultAsync();
    }

    public Task UpdateAsync(SalesMetrics metrics)
    {
        db.SalesMetrics.Update(metrics);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}
