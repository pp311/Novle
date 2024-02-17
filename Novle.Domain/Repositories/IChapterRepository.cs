using Novle.Domain.Entities;
using Novle.Domain.Repositories.Base;

namespace Novle.Domain.Repositories;

public interface IChapterRepository : IRepository<Chapter>
{
    IQueryable<Chapter> GetQuery(int bookId);
    Task<Chapter?> GetAsync(int chapterId, int bookId, CancellationToken cancellationToken = default);
    Task<double> GetMaxIndexAsync(int bookId, CancellationToken cancellationToken = default);
}