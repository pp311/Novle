using AutoMapper;
using AutoMapper.QueryableExtensions;
using Novle.Application.Common.Extensions;
using Novle.Application.Common.Models;
using Novle.Application.ViewModels;
using Novle.Application.ViewModels.Chapter.Requests;
using Novle.Application.ViewModels.Chapter.Responses;
using Novle.Domain.Entities;
using Novle.Domain.Exceptions;
using Novle.Domain.Repositories;
using Novle.Domain.Repositories.Base;

namespace Novle.Application.Services;

public class ChapterService : BaseService
{
    private readonly IChapterRepository _chapterRepository;
    private readonly IBookRepository _bookRepository;

    public ChapterService(IChapterRepository chapterRepository, 
                          IUnitOfWork unitOfWork, 
                          IMapper mapper,
                          IBookRepository bookRepository) : base(unitOfWork, mapper)
    {
        _chapterRepository = chapterRepository;
        _bookRepository = bookRepository;
    }
    
    public async Task DeleteChapterAsync(int id, int bookId)
    {
        var isBookExists = await _bookRepository.AnyAsync(bookId);
        EntityNotFoundException.ThrowIfFalse<Book>(isBookExists, bookId);
        
        var chapter = await _chapterRepository.GetByIdAsync(id);
        EntityNotFoundException.ThrowIfNull(chapter, id);
        
        _chapterRepository.Delete(chapter!);
        await UnitOfWork.SaveChangesAsync();
    }

    public async Task<PaginatedList<GetChapterTitleResponse>> GetChapterListAsync(
        int bookId, 
        PagingRequest request, 
        CancellationToken ct)
    {
        return await _chapterRepository.GetQuery(bookId)
            .OrderBy(c => c.Index, request.IsDescending)
            .ProjectTo<GetChapterTitleResponse>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, ct);
    }

    public async Task UpdateChapterAsync(int chapterId, int bookId, UpdateChapterRequest request)
    {
        var isBookExists = await _bookRepository.AnyAsync(bookId);
        EntityNotFoundException.ThrowIfFalse<Book>(isBookExists, bookId);

        var chapter = await _chapterRepository.GetByIdAsync(chapterId);
        EntityNotFoundException.ThrowIfNull(chapter, chapterId);
        
        chapter!.Update(request.Title, request.Content);
        _chapterRepository.Update(chapter);
        await UnitOfWork.SaveChangesAsync();
    }
    
    public async Task<GetChapterResponse> GetChapterAsync(
        int chapterId, 
        int bookId, 
        CancellationToken cancellationToken)
    {
        var chapter = await _chapterRepository.GetAsync(chapterId, bookId, cancellationToken);
        EntityNotFoundException.ThrowIfNull(chapter, chapterId);
        
        return _mapper.Map<GetChapterResponse>(chapter);
    }

    public async Task<int> CreateChapterAsync(int bookId, CreateChapterRequest request)
    {
        var isBookExists = await _bookRepository.AnyAsync(bookId);
        EntityNotFoundException.ThrowIfFalse<Book>(isBookExists, bookId);
        
        var chapter = await Chapter.CreateAsync(
            request.Title, 
            request.Content, 
            request.Index, 
            bookId, 
            _chapterRepository);
        
        _chapterRepository.Add(chapter);
        await UnitOfWork.SaveChangesAsync();
        
        return chapter.Id;
    }

    public async Task UpdateChapterIndexAsync(int chapterId, int bookId, UpdateChapterIndexRequest request)
    {
        var chapter = await _chapterRepository.GetAsync(chapterId, bookId);
        EntityNotFoundException.ThrowIfNull(chapter, chapterId);
        
        chapter!.UpdateIndex(request.Index);
        _chapterRepository.Update(chapter);
        await UnitOfWork.SaveChangesAsync();
    }
}