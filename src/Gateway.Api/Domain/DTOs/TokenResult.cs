namespace Gateway.Api.Domain.DTOs;

public record TokenResult(string Token, DateTime ExpiresAt);