using Microsoft.EntityFrameworkCore;
using Novle.Domain.Entities;
using Novle.Domain.Repositories;
using Novle.Infrastructure.Repositories.Base;

namespace Novle.Infrastructure.Repositories;

public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
{
	public AuthorRepository(NovleDbContext dbContext) : base(dbContext)
	{
	}
	
	public Task<bool> IsAuthorExistedAsync(string name, CancellationToken cancellationToken = default)
		=> GetQuery(a => a.Name == name).AnyAsync(cancellationToken);
}