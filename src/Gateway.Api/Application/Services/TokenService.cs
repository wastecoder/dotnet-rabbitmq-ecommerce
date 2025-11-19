using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gateway.Api.Domain.Entities;
using Gateway.Api.Domain.Interfaces;
using Gateway.Api.Presentation.Contracts.Responses;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.Api.Application.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    private readonly byte[] _key =
        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]
                               ?? throw new InvalidOperationException("Jwt:Key not configured"));

    public TokenResult GenerateToken(UserAccount user)
    {
        var expiresInMinutes = double.Parse(configuration["Jwt:ExpiresInMinutes"]!);
        var expiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes);

        var claims = CreateClaims(user);
        var credentials = CreateSigningCredentials();
        var token = CreateJwtToken(claims, expiresAt, credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenResult(tokenString, expiresAt);
    }

    private static List<Claim> CreateClaims(UserAccount user)
    {
        return new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(_key),
            SecurityAlgorithms.HmacSha256);
    }

    private JwtSecurityToken CreateJwtToken(
        IEnumerable<Claim> claims,
        DateTime expiresAt,
        SigningCredentials credentials)
    {
        return new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );
    }
}