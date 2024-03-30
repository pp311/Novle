using Carter;
using Novel.API.Filters;
using Novle.Application.Services;
using Novle.Application.ViewModels.Author.Requests;
using Novle.Application.ViewModels.Author.Responses;

namespace Novel.API.Endpoints;

public class AuthorModule : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/authors")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);
		
		// group.MapGet("", GetAuthorsAsync);
		group.MapGet("{id:int}", GetAuthorByIdAsync);
		group.MapPost("", CreateAuthorAsync);
		group.MapPut("{id:int}", UpdateAuthorAsync);
		group.MapDelete("{id:int}", DeleteAuthorAsync);
	}
	
	private static async Task<GetAuthorResponse> GetAuthorByIdAsync(
		int id,
		AuthorService authorService,
		CancellationToken cancellationToken)
		=> await authorService.GetAuthorByIdAsync(id, cancellationToken);

	private static async Task<int> CreateAuthorAsync(
		UpsertAuthorRequest request,
		AuthorService authorService)
		=> await authorService.CreateAuthorAsync(request);
	
	private static async Task UpdateAuthorAsync(
		int id,
		UpsertAuthorRequest request,
		AuthorService authorService)
		=> await authorService.UpdateAuthorAsync(id, request);
	
	private static async Task DeleteAuthorAsync(
		int id,
		AuthorService authorService)
		=> await authorService.DeleteAuthorAsync(id);
}