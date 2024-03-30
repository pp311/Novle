using Novle.Domain.Entities;
using Novle.Domain.Repositories.Base;

namespace Novle.Domain.Repositories;

public interface IAuthorRepository : IRepository<Author>
{
	Task<bool> IsAuthorExistedAsync(string name, CancellationToken cancellationToken = default);
}