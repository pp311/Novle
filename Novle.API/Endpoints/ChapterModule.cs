using Carter;
using Microsoft.AspNetCore.Mvc;
using Novle.Application.Common.Models;
using Novle.Application.Services;
using Novle.Application.ViewModels;
using Novle.Application.ViewModels.Chapter.Requests;
using Novle.Application.ViewModels.Chapter.Responses;

namespace Novel.API.Endpoints;

public class ChapterModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/books/{bookId:int}/chapters");

        group.MapGet("{chapterId:int}", GetChapterByIdAsync);
        group.MapGet("", GetChapterListAsync);
        group.MapPost("", CreateChapterAsync);
        group.MapPut("{chapterId:int}", UpdateChapterAsync);
        group.MapPut("{chapterId:int}/index", UpdateChapterIndexAsync);
        group.MapDelete("{chapterId:int}", DeleteChapterAsync); 
    }

    private static async Task UpdateChapterIndexAsync(
        [FromRoute] int bookId,
        [FromRoute] int chapterId, 
        UpdateChapterIndexRequest request,
        ChapterService chapterService)
        => await chapterService.UpdateChapterIndexAsync(chapterId, bookId, request); 

    private static async Task DeleteChapterAsync([FromRoute] int bookId, int chapterId, ChapterService chapterService)
        => await chapterService.DeleteChapterAsync(chapterId, bookId);

    private static async Task UpdateChapterAsync(
        [FromRoute] int bookId,
        [FromRoute] int chapterId,
        UpdateChapterRequest request,
        ChapterService chapterService)
        => await chapterService.UpdateChapterAsync(chapterId, bookId, request);

    private static async Task<int> CreateChapterAsync(
        [FromRoute] int bookId,
        CreateChapterRequest request,
        ChapterService chapterService)
        => await chapterService.CreateChapterAsync(bookId, request);
        

    private static async Task<PaginatedList<GetChapterTitleResponse>> GetChapterListAsync(
        [FromRoute] int bookId, 
        [AsParameters] PagingRequest request, 
        ChapterService chapterService,
        CancellationToken cancellationToken)
        => await chapterService.GetChapterListAsync(bookId, request, cancellationToken);
    
    private static async Task<GetChapterResponse> GetChapterByIdAsync(
        [FromRoute] int bookId, 
        [FromRoute] int chapterId,
        ChapterService chapterService,
        CancellationToken cancellationToken)
        => await chapterService.GetChapterAsync(chapterId, bookId, cancellationToken);
}