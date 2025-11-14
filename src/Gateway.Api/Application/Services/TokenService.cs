using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gateway.Api.Domain.DTOs;
using Gateway.Api.Domain.Enums;
using Gateway.Api.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.Api.Application.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public LoginResponse GenerateToken(string email, UserRole role)
    {
        var expiration = DateTime.UtcNow.AddMinutes(
            double.Parse(configuration["Jwt:ExpiresInMinutes"]!)
        );

        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, email),
            new(ClaimTypes.Role, role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginResponse(tokenString, expiration);
    }
}