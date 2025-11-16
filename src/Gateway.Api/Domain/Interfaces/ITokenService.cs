using Gateway.Api.Domain.DTOs;
using Gateway.Api.Domain.Entities;

namespace Gateway.Api.Domain.Interfaces;

public interface ITokenService
{
    TokenResult GenerateToken(UserAccount user);
}