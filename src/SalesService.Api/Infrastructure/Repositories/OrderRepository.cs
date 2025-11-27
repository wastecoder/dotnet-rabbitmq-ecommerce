using Microsoft.EntityFrameworkCore;
using SalesService.Api.Domain.Entities;
using SalesService.Api.Domain.Interfaces;
using SalesService.Api.Infrastructure.Database;

namespace SalesService.Api.Infrastructure.Repositories;

public class OrderRepository(SalesDbContext context) : IOrderRepository
{
    public async Task AddAsync(Order order)
    {
        await context.Orders.AddAsync(order);
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await context.Orders
            .Include(o => o.Items)
            .Where(o => !o.IsDeleted && o.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await context.Orders
            .Include(o => o.Items)
            .Where(o => !o.IsDeleted)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task UpdateAsync(Order order)
    {
        context.Orders.Update(order);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Order order)
    {
        order.SoftDelete();
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}