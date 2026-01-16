using Gateway.Api.Domain.Entities;
using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Presentation.Integration.Events.Sales;

namespace Gateway.Api.Application.Services;

public class ProductSalesStatsService(
    IProductSalesStatsRepository repository
) : IProductSalesStatsService
{
    public async Task IncrementProductSalesAsync(OrderCreatedEvent e)
    {
        foreach (var item in e.Items)
        {
            var stats = await repository.GetByProductIdAsync(item.ProductId);

            if (stats is null)
            {
                stats = new ProductSalesStats(
                    item.ProductId,
                    item.ProductName,
                    item.Quantity
                );

                await repository.AddAsync(stats);
            }
            else
            {
                stats.IncreaseSales(item.Quantity);
                await repository.UpdateAsync(stats);
            }
        }

        await repository.SaveChangesAsync();
    }
}