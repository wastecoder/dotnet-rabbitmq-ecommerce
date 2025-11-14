using Gateway.Api.Domain.DTOs;
using Gateway.Api.Domain.Enums;

namespace Gateway.Api.Domain.Interfaces;

public interface ITokenService
{
    LoginResponse GenerateToken(string email, UserRole role);
}