using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalesService.Api.Presentation.Contracts.Requests;
using SalesService.Api.Presentation.Contracts.Responses;
using SalesService.Api.Domain.Interfaces;
using SalesService.Api.Presentation.Factories;

namespace SalesService.Api.Presentation.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(IOrderService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAll()
    {
        var orders = await service.GetAllOrdersAsync();
        return Ok(ApiResponseFactory.Success(mapper.Map<IEnumerable<OrderResponse>>(orders)));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResponse>> GetById(Guid id)
    {
        var order = await service.GetOrderByIdAsync(id);
        return Ok(ApiResponseFactory.Success(mapper.Map<OrderResponse>(order)));
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] CreateOrderRequest request)
    {
        var order = await service.CreateOrderAsync(request);

        var response = mapper.Map<OrderResponse>(order);
        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            ApiResponseFactory.Created(response));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<OrderResponse>> Update(Guid id, [FromBody] CreateOrderRequest request)
    {
        var updated = await service.UpdateOrderAsync(id, request);
        return Ok(ApiResponseFactory.Updated(mapper.Map<OrderResponse>(updated)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.SoftDeleteOrderAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:guid}/confirm")]
    public async Task<ActionResult<OrderResponse>> Confirm(Guid id)
    {
        var confirmed = await service.ConfirmOrderAsync(id);
        return Ok(ApiResponseFactory.Updated(mapper.Map<OrderResponse>(confirmed)));
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<ActionResult<OrderResponse>> Cancel(Guid id)
    {
        var cancelled = await service.CancelOrderAsync(id);
        return Ok(ApiResponseFactory.Updated(mapper.Map<OrderResponse>(cancelled)));
    }
}