using Novle.Domain.Entities;
using Novle.Domain.Repositories.Base;

namespace Novle.Domain.Repositories;

public interface IBookRepository : IRepository<Book>
{
    IQueryable<Book> Search(string? search);
}