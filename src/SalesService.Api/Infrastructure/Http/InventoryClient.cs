using System.Net;
using System.Net.Http.Json;
using SalesService.Api.Domain.Exceptions;
using SalesService.Api.Domain.Interfaces;
using SalesService.Api.Domain.Messages;
using SalesService.Api.Presentation.Contracts.Requests;
using SalesService.Api.Presentation.Contracts.Responses;

namespace SalesService.Api.Infrastructure.Http;

public class InventoryClient(HttpClient httpClient) : IInventoryClient
{
    private const string BasePath = "api/stock";

    public async Task<bool> CheckStockAsync(OrderItemStockCheckDto item)
    {
        var url = $"{BasePath}/{item.ProductId}";

        var response = await httpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException($"Product {item.ProductId} not found in Inventory.");

        if (!response.IsSuccessStatusCode)
            throw new ExternalServiceException("Failed to contact InventoryService.");

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<StockAvailabilityResponse>>()
                          ?? throw new ExternalServiceException("Invalid response from InventoryService.");

        return apiResponse.Data.AvailableQuantity >= item.Quantity;
    }

    public async Task<StockUpdatedResponse> DecreaseStockAsync(OrderItemStockUpdateDto item)
    {
        var url = $"{BasePath}/{item.ProductId}/decrease";

        var requestBody = new StockUpdateRequest(item.Quantity);

        var response = await httpClient.PostAsJsonAsync(url, requestBody);

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException($"Product {item.ProductId} not found in Inventory.");

        if (!response.IsSuccessStatusCode)
            throw new ExternalServiceException("Failed to decrease stock in InventoryService.");

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<StockUpdatedResponse>>()
                          ?? throw new ExternalServiceException("Invalid response from InventoryService.");

        return apiResponse.Data;
    }

    public async Task<StockUpdatedResponse> IncreaseStockAsync(OrderItemStockUpdateDto item)
    {
        var url = $"{BasePath}/{item.ProductId}/increase";

        var requestBody = new StockUpdateRequest(item.Quantity);

        var response = await httpClient.PostAsJsonAsync(url, requestBody);

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException($"Product {item.ProductId} not found in Inventory.");

        if (!response.IsSuccessStatusCode)
            throw new ExternalServiceException("Failed to increase stock in InventoryService.");

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<StockUpdatedResponse>>()
                          ?? throw new ExternalServiceException("Invalid response from InventoryService.");

        return apiResponse.Data;
    }

    public async Task<ProductResponse> GetProductByIdAsync(Guid id)
    {
        var url = $"api/products/{id}";

        var response = await httpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException($"Product {id} not found in Inventory.");

        if (!response.IsSuccessStatusCode)
            throw new ExternalServiceException("Failed to contact InventoryService.");

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ProductResponse>>()
                          ?? throw new ExternalServiceException("Invalid response from InventoryService.");

        return apiResponse.Data;
    }
}