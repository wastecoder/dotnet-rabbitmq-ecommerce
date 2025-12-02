using Gateway.Api.Domain.Entities;

namespace Gateway.Api.Domain.Interfaces;

public interface ISalesMetricsRepository
{
    Task AddAsync(SalesMetrics metrics);
    Task<SalesMetrics?> GetAsync();
    Task UpdateAsync(SalesMetrics metrics);
    Task SaveChangesAsync();
}