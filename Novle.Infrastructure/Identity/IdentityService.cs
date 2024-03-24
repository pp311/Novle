using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Novle.Application.Common.Interfaces;
using Novle.Domain.Entities;
using Novle.Domain.Enums;
using Novle.Domain.Repositories.Base;

namespace Novle.Infrastructure.Identity;

public class IdentityService(
    UserManager<ApplicationUser> userManager,
    IMapper mapper,
    IRepository<RefreshToken> refreshTokenRepo,
    IUnitOfWork unitOfWork) : IIdentityService
{
    public async Task<User?> GetAsync(int userId)
        => mapper.Map<User>(await userManager.FindByIdAsync(userId.ToString()));

    public async Task<User?> GetAsync(string identifier)
    {
        if (identifier.Contains("@"))
            return mapper.Map<User>(await userManager.FindByEmailAsync(identifier));

        return mapper.Map<User>(await userManager.FindByNameAsync(identifier));
    }

    public async Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime expires)
    {
        var token = new RefreshToken
        {
            Token = refreshToken,
            UserId = userId,
            Expires = expires
        };
        refreshTokenRepo.Add(token);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> CreateUserAsync(User user, string password)
    {
        var applicationUser = mapper.Map<ApplicationUser>(user);
        var result = await userManager.CreateAsync(applicationUser, password);
        if (result.Succeeded)
            result = await userManager.AddToRoleAsync(applicationUser, AppRole.User.ToString());

        return result.Succeeded;
    }

    public async Task<bool> ValidatePasswordAsync(int userId, string password)
    {
        var appUser = await userManager.FindByIdAsync(userId.ToString());
        return await userManager.CheckPasswordAsync(appUser!, password);
    }
}
