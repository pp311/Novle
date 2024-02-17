using Carter;
using Novel.API.Filters;
using Novle.Application.Common.Models;
using Novle.Application.Services;
using Novle.Application.ViewModels.Book.Requests;
using Novle.Application.ViewModels.Book.Responses;

namespace Novel.API.Endpoints;

public class BookModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/books")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);

        group.MapGet("{id:int}", GetBookByIdAsync);
        group.MapGet("", GetBooksAsync);
        group.MapPost("", CreateBookAsync);
        group.MapPut("{id:int}", UpdateBookAsync);
        group.MapPut("{id:int}/view-count", UpdateViewCountAsync);
        group.MapDelete("{id:int}", DeleteBookAsync);
    }

    private static async Task<GetBookInfoResponse> GetBookByIdAsync(
        int id,
        BookService bookService,
        CancellationToken cancellationToken)
        => await bookService.GetBookByIdAsync(id, cancellationToken);
    
    private static async Task<PaginatedList<GetBookInfoResponse>> GetBooksAsync(
        [AsParameters] GetBooksRequest request, 
        BookService bookService, 
        CancellationToken cancellationToken)
        => await bookService.GetBooksAsync(request, cancellationToken);

    private static async Task<int> CreateBookAsync(
        UpsertBookRequest request, 
        BookService bookService) 
        => await bookService.CreateBookAsync(request);
    
    private static async Task UpdateBookAsync(
        int id, 
        UpsertBookRequest request, 
        BookService bookService)
        => await bookService.UpdateBookAsync(id, request);
    
    private static async Task UpdateViewCountAsync(int id, BookService bookService)
        => await bookService.UpdateViewCountAsync(id);
    
    private static async Task DeleteBookAsync(
        int id, 
        BookService bookService) 
         => await bookService.DeleteBookAsync(id);
}