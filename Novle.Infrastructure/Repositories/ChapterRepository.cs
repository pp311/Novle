using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;
using Novle.Domain.Entities;
using Novle.Domain.Repositories;
using Novle.Infrastructure.Repositories.Base;

namespace Novle.Infrastructure.Repositories;

public class ChapterRepository : RepositoryBase<Chapter>, IChapterRepository
{
    public ChapterRepository(NovleDbContext dbContext) : base(dbContext)
    {
    }

    public IQueryable<Chapter> GetQuery(int bookId)
        => GetQuery(c => c.BookId == bookId);

    public Task<Chapter?> GetAsync(int chapterId, int bookId, CancellationToken cancellationToken)
        => GetAsync(c => c.BookId == bookId && c.Id == chapterId, cancellationToken);

    public Task<double> GetMaxIndexAsync(int bookId, CancellationToken cancellationToken = default)
        => TryAsync(GetQuery(bookId).MaxAsync(c => c.Index, cancellationToken)).Match(value => value, 1.0);
}