using Novle.Domain.Entities;

namespace Novle.Application.Common.Interfaces;

public interface IIdentityService : IInfrastructureService
{
    Task<User?> GetAsync(int userId);
    Task<User?> GetAsync(string email);
    Task<bool> ValidatePasswordAsync(int userId, string password);
    Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime expires);
    Task<bool> CreateUserAsync(User user, string password);
}
