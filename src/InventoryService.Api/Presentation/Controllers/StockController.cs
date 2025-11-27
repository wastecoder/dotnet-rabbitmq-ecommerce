using AutoMapper;
using InventoryService.Api.Domain.Interfaces;
using InventoryService.Api.Presentation.Contracts.Requests;
using InventoryService.Api.Presentation.Contracts.Responses;
using InventoryService.Api.Presentation.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Api.Presentation.Controllers;

[ApiController]
[Route("api/stock/{id:guid}")]
public class StockController(IStockService service, IMapper mapper) : ControllerBase
{
    // [Authorize(Policy = "USER")]
    [HttpGet]
    public async Task<ActionResult<StockAvailabilityResponse>> GetAvailability(Guid id)
    {
        var product = await service.GetAvailabilityAsync(id);
        return Ok(ApiResponseFactory.Success(mapper.Map<StockAvailabilityResponse>(product)));
    }
    
    // [Authorize(Policy = "ADMIN")]
    [HttpPost("decrease")]
    public async Task<ActionResult<StockUpdatedResponse>> Decrease(
        Guid id,
        [FromBody] StockUpdateRequest request)
    {
        var (updated, oldQuantity) = await service.DecreaseStockAsync(id, request.Quantity);
        var result = (product: updated, oldQuantity);
        return Ok(ApiResponseFactory.Updated(mapper.Map<StockUpdatedResponse>(result)));
    }

    // [Authorize(Policy = "ADMIN")]
    [HttpPost("increase")]
    public async Task<ActionResult<StockUpdatedResponse>> Increase(
        Guid id,
        [FromBody] StockUpdateRequest request)
    {
        var (updated, oldQuantity) = await service.IncreaseStockAsync(id, request.Quantity);
        var result = (product: updated, oldQuantity);
        return Ok(ApiResponseFactory.Updated(mapper.Map<StockUpdatedResponse>(result)));
    }
}