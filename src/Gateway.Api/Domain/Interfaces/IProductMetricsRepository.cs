using Gateway.Api.Domain.Entities;

namespace Gateway.Api.Domain.Interfaces;

public interface IProductMetricsRepository
{
    Task AddAsync(ProductMetrics metrics);
    Task<ProductMetrics?> GetByProductIdAsync(Guid productId);
    Task UpdateAsync(ProductMetrics metrics);
    Task SaveChangesAsync();
}