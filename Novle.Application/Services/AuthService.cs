using AutoMapper;
using Novle.Application.Common.Interfaces;
using Novle.Application.ViewModels.Auth.Requests;
using Novle.Application.ViewModels.Auth.Responses;
using Novle.Domain.Entities;
using Novle.Domain.Exceptions;
using Novle.Domain.Repositories.Base;

namespace Novle.Application.Services;

public class AuthService(IUnitOfWork unitOfWork,
    IMapper mapper,
    IIdentityService identityService,
    TokenService tokenService) : BaseService(unitOfWork, mapper)
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await identityService.GetAsync(request.Identifier)
            ?? throw new AuthException();

        var isPasswordValid = await identityService.ValidatePasswordAsync(user.Id, request.Password);
        AuthException.ThrowIfFalse(isPasswordValid);

        var token = tokenService.GenerateToken(user);
        var (refreshToken, expires) = tokenService.GenerateRefreshToken();

        await identityService.SaveRefreshTokenAsync(user.Id, refreshToken, expires);

        return new LoginResponse(token, refreshToken);
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var isEmailTaken = await identityService.GetAsync(request.Email) != null;
        if (isEmailTaken)
            throw new AppException("Email already taken");

        var isUserNameTaken = await identityService.GetAsync(request.UserName) != null;
        if (isUserNameTaken)
            throw new AppException("Username already taken");

        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            FullName = request.FullName
        };

        await unitOfWork.BeginTransactionAsync();

        try
        {
            var isSucceeded = await identityService.CreateUserAsync(user, request.Password);
            if (!isSucceeded)
                throw new AppException("Failed to create user");

            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}
