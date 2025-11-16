using Gateway.Api.Domain.DTOs;
using Gateway.Api.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Login with email and password. Returns JWT token on success.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var (success, tokenResult, error) = await authService.LoginAsync(request, cancellationToken);

        if (!success)
            return Unauthorized(error);

        return Ok(tokenResult);
    }
}