using AutoMapper;
using InventoryService.Api.Domain.Interfaces;
using InventoryService.Api.Presentation.Contracts.Requests;
using InventoryService.Api.Presentation.Contracts.Responses;
using InventoryService.Api.Presentation.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Api.Presentation.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(IProductService service, IMapper mapper) : ControllerBase
{
    // [Authorize(Policy = "USER")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
    {
        var products = await service.GetAllAsync();
        return Ok(ApiResponseFactory.Success(mapper.Map<IEnumerable<ProductResponse>>(products)));
    }

    // [Authorize(Policy = "USER")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id)
    {
        var product = await service.GetByIdAsync(id);
        return Ok(ApiResponseFactory.Success(mapper.Map<ProductResponse>(product)));
    }

    // [Authorize(Policy = "ADMIN")]
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create([FromBody] ProductRequest request)
    {
        var product = await service.CreateAsync(request);

        var response = mapper.Map<ProductResponse>(product);
        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            ApiResponseFactory.Created(response));
    }

    // [Authorize(Policy = "ADMIN")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> Update(Guid id, [FromBody] ProductRequest request)
    {
        var updated = await service.UpdateAsync(id, request);
        return Ok(ApiResponseFactory.Updated(mapper.Map<ProductResponse>(updated)));
    }

    // [Authorize(Policy = "ADMIN")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.SoftDeleteAsync(id);
        return NoContent();
    }
}