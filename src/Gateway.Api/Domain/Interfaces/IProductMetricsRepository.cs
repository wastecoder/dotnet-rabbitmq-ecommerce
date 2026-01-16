using Gateway.Api.Domain.Entities;

namespace Gateway.Api.Domain.Interfaces;

public interface IProductMetricsRepository
{
    Task AddAsync(ProductMetrics metrics);
    Task<ProductMetrics?> GetByProductIdAsync(Guid productId);
    Task<IReadOnlyList<ProductMetrics>> GetAllAsync();
    Task UpdateAsync(ProductMetrics metrics);
    Task SaveChangesAsync();
}