using Carter;
using Novle.Application.Services;
using Novle.Application.ViewModels.Auth.Requests;
using Novle.Application.ViewModels.Auth.Responses;

namespace Novel.API.Endpoints;

public class AuthModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("register", RegisterAsync);
        group.MapPost("login", LoginAsync);
        // group.MapPost("refresh-token", RefreshTokenAsync);
        // group.MapPost("revoke-token", RevokeTokenAsync);
    }

    private static async Task RegisterAsync(
        RegisterRequest request,
        AuthService authService)
        => await authService.RegisterAsync(request);

    private static async Task<LoginResponse> LoginAsync(
        LoginRequest request,
        AuthService authService)
        => await authService.LoginAsync(request);

    // private static async Task<AuthenticateResponse> RefreshTokenAsync(
    //     RefreshTokenRequest request,
    //     AuthService authService)
    //     => await authService.RefreshTokenAsync(request);

    // private static async Task RevokeTokenAsync(
    //     RevokeTokenRequest request,
    //     AuthService authService)
    //     => await authService.RevokeTokenAsync(request);
}
