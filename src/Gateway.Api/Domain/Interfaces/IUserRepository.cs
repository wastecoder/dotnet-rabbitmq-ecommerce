using Gateway.Api.Domain.Entities;

namespace Gateway.Api.Domain.Interfaces;

public interface IUserRepository
{
    Task<UserAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}