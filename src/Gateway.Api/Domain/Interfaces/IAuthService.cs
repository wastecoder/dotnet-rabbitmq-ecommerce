using Gateway.Api.Presentation.Contracts.Requests;
using Gateway.Api.Presentation.Contracts.Responses;
using Gateway.Api.Presentation.Models;

namespace Gateway.Api.Domain.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Attempt to authenticate a user using email + password.
    /// Returns an AuthResult containing a success flag, a TokenResult if successful, or an ErrorResponse if failed.
    /// </summary>
    Task<ApiResponse<TokenResult>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}