using Gateway.Api.Domain.Entities;
using Gateway.Api.Presentation.Contracts.Responses;

namespace Gateway.Api.Domain.Interfaces;

public interface ITokenService
{
    TokenResult GenerateToken(UserAccount user);
}