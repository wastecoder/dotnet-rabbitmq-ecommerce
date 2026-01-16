using Gateway.Api.Presentation.Contracts.Responses;

namespace Gateway.Api.Domain.Interfaces;

public interface IMetricsOrchestrator
{
    Task<MetricsDashboardResponse> GetDashboardAsync(int topSellingCount);
}