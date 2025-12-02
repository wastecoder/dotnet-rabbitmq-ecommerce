using Gateway.Api.Domain.Entities;

namespace Gateway.Api.Domain.Interfaces;

public interface IProductSalesStatsRepository
{
    Task AddAsync(ProductSalesStats stats);
    Task<ProductSalesStats?> GetByProductIdAsync(Guid productId);
    Task UpdateAsync(ProductSalesStats stats);
    Task SaveChangesAsync();
}