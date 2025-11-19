namespace Gateway.Api.Presentation.Contracts.Responses;

public record TokenResult(string Token, DateTime ExpiresAt);