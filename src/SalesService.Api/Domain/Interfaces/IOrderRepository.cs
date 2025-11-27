using SalesService.Api.Domain.Entities;

namespace SalesService.Api.Domain.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task UpdateAsync(Order order);
    Task DeleteAsync(Order order);
    Task SaveChangesAsync();
}