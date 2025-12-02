using Gateway.Api.Domain.Entities;

namespace Gateway.Api.Domain.Interfaces;

public interface IStockAlertRepository
{
    Task AddAsync(StockAlert alert);
    Task<List<StockAlert>> GetSinceAsync(DateTime sinceUtc);
    Task<List<StockAlert>> GetByProductIdAsync(Guid productId);
    Task SaveChangesAsync();
}