using Novle.Domain.Entities;
using Novle.Domain.Repositories;
using Novle.Infrastructure.Repositories.Base;

namespace Novle.Infrastructure.Repositories;

public class BookRepository : RepositoryBase<Book>, IBookRepository
{
    public BookRepository(NovleDbContext dbContext) : base(dbContext)
    {
    }
    
    public IQueryable<Book> Search(string? search)
        => string.IsNullOrWhiteSpace(search) 
            ? GetQuery() 
            : GetQuery(b => b.Title.Contains(search) || b.Author.Name.Contains(search));
}