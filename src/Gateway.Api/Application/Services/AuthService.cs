using Gateway.Api.Domain.Entities;
using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Presentation.Contracts.Requests;
using Gateway.Api.Presentation.Contracts.Responses;
using Gateway.Api.Presentation.Models;
using Microsoft.AspNetCore.Identity;

namespace Gateway.Api.Application.Services;

/// <summary>
/// Authentication service responsible for validating credentials and issuing tokens.
/// </summary>
public class AuthService(
    IUserRepository userRepository,
    ITokenService tokenService) : IAuthService
{
    private readonly PasswordHasher<UserAccount> _passwordHasher = new();

    public async Task<ApiResponse<TokenResult>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (user is null)
            throw new UnauthorizedAccessException("Invalid credentials.");

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid credentials.");

        var tokenResult = tokenService.GenerateToken(user);

        return new ApiResponse<TokenResult>(tokenResult);
    }
}