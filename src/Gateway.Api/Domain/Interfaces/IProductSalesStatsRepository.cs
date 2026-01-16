using Gateway.Api.Domain.Entities;
using Gateway.Api.Presentation.Contracts.Responses;

namespace Gateway.Api.Domain.Interfaces;

public interface IProductSalesStatsRepository
{
    Task AddAsync(ProductSalesStats stats);
    Task<ProductSalesStats?> GetByProductIdAsync(Guid productId);
    Task<IReadOnlyList<TopSellingProductDto>> GetTopSellingAsync(int topCount);
    Task UpdateAsync(ProductSalesStats stats);
    Task SaveChangesAsync();
}