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

        var data = await ReadResultOrThrow<StockAvailabilityResponse>(response);

        if (data.AvailableQuantity < 0)
            throw new ExternalServiceException("Inventory returned an invalid stock quantity.");

        return data.AvailableQuantity >= item.Quantity;
    }

    public async Task<StockUpdatedResponse> DecreaseStockAsync(OrderItemStockUpdateDto item)
    {
        var url = $"{BasePath}/{item.ProductId}/decrease";

        var requestBody = new StockUpdateRequest(item.Quantity);

        var response = await httpClient.PostAsJsonAsync(url, requestBody);

        return await ReadResultOrThrow<StockUpdatedResponse>(response);
    }

    public async Task<StockUpdatedResponse> IncreaseStockAsync(OrderItemStockUpdateDto item)
    {
        var url = $"{BasePath}/{item.ProductId}/increase";

        var requestBody = new StockUpdateRequest(item.Quantity);

        var response = await httpClient.PostAsJsonAsync(url, requestBody);

        return await ReadResultOrThrow<StockUpdatedResponse>(response);
    }

    public async Task<ProductResponse> GetProductByIdAsync(Guid id)
    {
        var url = $"api/products/{id}";

        var response = await httpClient.GetAsync(url);

        return await ReadResultOrThrow<ProductResponse>(response);
    }


    private static async Task<T> ReadResultOrThrow<T>(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException("Resource not found in Inventory.");

        if (!response.IsSuccessStatusCode)
            throw new ExternalServiceException("Failed to contact InventoryService.");

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
        if (apiResponse is null)
            throw new ExternalServiceException("Invalid response from InventoryService.");


        if (apiResponse.Data is null)
            throw new ExternalServiceException("InventoryService returned an empty result.");

        return apiResponse.Data;
    }
}