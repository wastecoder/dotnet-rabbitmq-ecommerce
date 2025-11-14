using Gateway.Api.Domain.DTOs;
using Gateway.Api.Domain.Entities;
using Gateway.Api.Domain.Enums;
using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(ITokenService tokenService, AppDbContext db) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await db.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user is null)
            return Unauthorized(new ErrorResponse("Invalid credentials"));

        var hasher = new PasswordHasher<UserAccount>();
        var passwordResult = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (passwordResult == PasswordVerificationResult.Failed)
            return Unauthorized(new ErrorResponse("Invalid credentials"));

        var tokenResult = tokenService.GenerateToken(user.Email, user.Role);

        return Ok(new LoginResponse(tokenResult.Token, tokenResult.ExpiresAt));

    }

    [HttpGet("admin")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public IActionResult TestAdmin()
    {
        return Ok(new ErrorResponse("Logged in as ADMINISTRATOR!"));
    }

    [HttpGet("user")]
    [Authorize(Roles = nameof(UserRole.User))]
    public IActionResult TestUser()
    {
        return Ok(new ErrorResponse("Logged in as USER!"));
    }
}