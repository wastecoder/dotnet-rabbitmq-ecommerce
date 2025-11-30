namespace SalesService.Api.Infrastructure.Configurations;

public class RabbitMqSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ExchangeName { get; set; } = string.Empty;
    public bool Durable { get; set; }
    public int RetryCount { get; set; }
}