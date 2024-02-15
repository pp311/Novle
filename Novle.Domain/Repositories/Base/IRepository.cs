using System.Linq.Expressions;
using Novle.Domain.Entities.Base;

namespace Novle.Domain.Repositories.Base;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
    IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>>? predicate = null);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity); 
    void DeleteRange(IEnumerable<TEntity> entities);
    Task<bool> AnyAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> IsAllExistAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}