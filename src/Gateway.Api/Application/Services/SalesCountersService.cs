using Gateway.Api.Domain.Entities;
using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Presentation.Integration.Events.Sales;

namespace Gateway.Api.Application.Services;

public class SalesCountersService(
    ISalesMetricsRepository repository
) : ISalesCountersService
{
    public async Task HandleOrderConfirmedAsync(OrderConfirmedEvent e)
    {
        var metrics = await repository.GetAsync();

        if (metrics is null)
        {
            metrics = new SalesMetrics();

            metrics.UpdateCounters(
                total: 1,
                confirmed: 1,
                canceled: 0,
                lastOrderAt: e.ConfirmedAt
            );

            await repository.AddAsync(metrics);
        }
        else
        {
            metrics.UpdateCounters(
                total: metrics.TotalSales + 1,
                confirmed: metrics.ConfirmedSales + 1,
                canceled: metrics.CanceledSales,
                lastOrderAt: e.ConfirmedAt
            );

            await repository.UpdateAsync(metrics);
        }

        await repository.SaveChangesAsync();
    }
}