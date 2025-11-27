using SalesService.Api.Domain.Entities;
using SalesService.Api.Presentation.Contracts.Requests;

namespace SalesService.Api.Domain.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(CreateOrderRequest request);
    Task<Order> GetOrderByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order> UpdateOrderAsync(Guid id, CreateOrderRequest request);
    Task SoftDeleteOrderAsync(Guid id);
}