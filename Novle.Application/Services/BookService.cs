using AutoMapper;
using AutoMapper.QueryableExtensions;
using Novle.Application.Common.Extensions;
using Novle.Application.Common.Models;
using Novle.Application.Helpers;
using Novle.Application.ViewModels.Book.Requests;
using Novle.Application.ViewModels.Book.Responses;
using Novle.Domain.Entities;
using Novle.Domain.Exceptions;
using Novle.Domain.Repositories;
using Novle.Domain.Repositories.Base;

namespace Novle.Application.Services;

public class BookService : BaseService
{
    private readonly IBookRepository _bookRepository;
    private readonly IRepository<Author> _authorRepository;
    private readonly IRepository<Genre> _genreRepository;
    
    public BookService(IUnitOfWork unitOfWork,
                       IBookRepository bookRepository,
                       IMapper mapper,
                       IRepository<Genre> genreRepository,
                       IRepository<Author> authorRepository) : base(unitOfWork, mapper)
    {
        _bookRepository = bookRepository;
        _genreRepository = genreRepository;
        _authorRepository = authorRepository;
    }
    
    public async Task<GetBookInfoResponse> GetBookByIdAsync(int id, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken);
        EntityNotFoundException.ThrowIfNull(book, id);
        
        return _mapper.Map<GetBookInfoResponse>(book);
    } 
    
    public async Task<PaginatedList<GetBookInfoResponse>> GetBooksAsync(GetBooksRequest request, CancellationToken ct)
    {
        return await _bookRepository
            .Search(request.Search)
            .WhereIf(request.AuthorId.HasValue, x => x.AuthorId == request.AuthorId!.Value)
            .WhereIf(request.GenreId.HasValue, x => x.Genres.Any(bg => bg.Id == request.GenreId!.Value))
            .OrderBy(GetOrderByField(request.SortBy), request.IsDescending)
            .ProjectTo<GetBookInfoResponse>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, ct);
    }
    
    public async Task<int> CreateBookAsync(UpsertBookRequest request, CancellationToken cancellationToken)
    {
        var isAuthorExists = await _authorRepository.AnyAsync(request.AuthorId, cancellationToken);
        EntityNotFoundException.ThrowIfFalse<Author>(isAuthorExists, request.AuthorId);

        var isAllGenresExists = await _genreRepository.IsAllExistAsync(request.GenreIds, cancellationToken);
        EntityNotFoundException.ThrowIfFalse<Genre>(isAllGenresExists);
        
        var book = Book.Create(
            request.Title, 
            request.Description, 
            request.CoverUrl, 
            request.AuthorId, 
            request.GenreIds);
        
        _bookRepository.Add(book);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        
        return book.Id;
    }

    public async Task UpdateBookAsync(
        int id, 
        UpsertBookRequest request,
        CancellationToken cancellationToken)
    {

        var book = await _bookRepository.GetByIdAsync(id, cancellationToken);
        EntityNotFoundException.ThrowIfNull(book, id);
        
        var isAuthorExists = await _authorRepository.AnyAsync(request.AuthorId, cancellationToken);
        EntityNotFoundException.ThrowIfFalse<Author>(isAuthorExists, request.AuthorId);

        var isAllGenresExists = await _genreRepository.IsAllExistAsync(request.GenreIds, cancellationToken);
        EntityNotFoundException.ThrowIfFalse<Genre>(isAllGenresExists);
        
        book!.Update(
            request.Title, 
            request.Description, 
            request.CoverUrl, 
            request.AuthorId, 
            request.GenreIds);
        
        _bookRepository.Update(book);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteBookAsync(int id, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken);
        EntityNotFoundException.ThrowIfNull(book, id);

        book!.Delete();
        _bookRepository.Delete(book);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateViewCountAsync(int id, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken);
        EntityNotFoundException.ThrowIfNull(book, id);
        
        book!.IncreaseViewCount();
        _bookRepository.Update(book);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    private IOrderByField GetOrderByField(BookSortByOption? option)
    {
        return option switch
        {
            BookSortByOption.Id 
                => new OrderByField<Book, int>(x => x.Id),
            BookSortByOption.Title 
                => new OrderByField<Book, string>(x => x.Title),
            BookSortByOption.ViewCount 
                => new OrderByField<Book, int>(x => x.ViewCount),
            BookSortByOption.RatingCount 
                => new OrderByField<Book, int>(x => x.Reviews.Count),
            BookSortByOption.RatingScore 
                => new OrderByField<Book, double>(x => x.Reviews.DefaultIfEmpty().Average(r => r != null ? r.Rating : 0)),
            _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
        };
    }
}