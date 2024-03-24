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

public class BookService(
    IUnitOfWork unitOfWork,
    IBookRepository bookRepository,
    IMapper mapper,
    IRepository<Genre> genreRepository,
    IRepository<Author> authorRepository)
    : BaseService(unitOfWork, mapper)
{
    public async Task<GetBookInfoResponse> GetBookByIdAsync(int id, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetByIdAsync(id, cancellationToken);
        EntityNotFoundException.ThrowIfNull(book, id);

        return Mapper.Map<GetBookInfoResponse>(book);
    }

    public async Task<PaginatedList<GetBookInfoResponse>> GetBooksAsync(GetBooksRequest request, CancellationToken ct)
    {
        return await bookRepository
            .Search(request.Search)
            .WhereIf(request.AuthorId.HasValue, x => x.AuthorId == request.AuthorId!.Value)
            .WhereIf(request.GenreId.HasValue, x => x.Genres.Any(bg => bg.Id == request.GenreId!.Value))
            .OrderBy(GetOrderByField(request.SortBy), request.IsDescending)
            .ProjectTo<GetBookInfoResponse>(Mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, ct);
    }

    public async Task<int> CreateBookAsync(UpsertBookRequest request)
    {
        var isAuthorExists = await authorRepository.AnyAsync(request.AuthorId);
        EntityNotFoundException.ThrowIfFalse<Author>(isAuthorExists, request.AuthorId);

        var isAllGenresExists = await genreRepository.IsAllExistAsync(request.GenreIds);
        EntityNotFoundException.ThrowIfFalse<Genre>(isAllGenresExists);

        var book = Book.Create(
            request.Title,
            request.Description,
            request.CoverUrl,
            request.AuthorId,
            request.GenreIds);

        bookRepository.Add(book);
        await UnitOfWork.SaveChangesAsync();

        return book.Id;
    }

    public async Task UpdateBookAsync(int id, UpsertBookRequest request)
    {
        var book = await bookRepository.GetByIdAsync(id);
        EntityNotFoundException.ThrowIfNull(book, id);

        var isAuthorExists = await authorRepository.AnyAsync(request.AuthorId);
        EntityNotFoundException.ThrowIfFalse<Author>(isAuthorExists, request.AuthorId);

        var isAllGenresExists = await genreRepository.IsAllExistAsync(request.GenreIds);
        EntityNotFoundException.ThrowIfFalse<Genre>(isAllGenresExists);

        book!.Update(
            request.Title,
            request.Description,
            request.CoverUrl,
            request.AuthorId,
            request.GenreIds);

        bookRepository.Update(book);
        await UnitOfWork.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await bookRepository.GetByIdAsync(id);
        EntityNotFoundException.ThrowIfNull(book, id);

        book!.Delete();
        bookRepository.Delete(book);
        await UnitOfWork.SaveChangesAsync();
    }

    public async Task UpdateViewCountAsync(int id)
    {
        var book = await bookRepository.GetByIdAsync(id);
        EntityNotFoundException.ThrowIfNull(book, id);

        book!.IncreaseViewCount();
        bookRepository.Update(book);
        await UnitOfWork.SaveChangesAsync();
    }

    private static IOrderByField GetOrderByField(BookSortByOption? option)
    {
        // TODO: view count by day, week, month, year
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
            BookSortByOption.LatestUploaded
                => new OrderByField<Book, DateTime>(x => x.CreatedOn!.Value),
            BookSortByOption.LatestUpdated
                => new OrderByField<Book, DateTime>(x => x.UpdatedOn!.Value),
            BookSortByOption.ArchivedCount
                => new OrderByField<Book, int>(x => x.Archives.Count),
            _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
        };
    }
}
