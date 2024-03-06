using Carter;
using Novle.Application.Services;
using Novle.Application.ViewModels.Genre.Requests;

namespace Novel.API.Endpoints;

public class GenreModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/genres");

        group.MapGet("", GetGenreListAsync);
    }
    
    private static async Task<List<GetGenreResponse>> GetGenreListAsync(
        GenreService genreService,
        CancellationToken cancellationToken)
        => await genreService.GetGenreListAsync(cancellationToken);
}