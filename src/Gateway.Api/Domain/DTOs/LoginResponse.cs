namespace Gateway.Api.Domain.DTOs;

public record LoginResponse(string Token, DateTime ExpiresAt);