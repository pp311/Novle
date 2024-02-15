using Novle.Domain.Repositories.Base;

namespace Novle.Infrastructure.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly NovleDbContext _context;

    public UnitOfWork(NovleDbContext context)
    {
        _context = context;
    }
    
    public Task<int> SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);

    public Task BeginTransactionAsync(CancellationToken ct) => _context.Database.BeginTransactionAsync(ct);

    public Task CommitAsync(CancellationToken ct) => _context.Database.CommitTransactionAsync(ct);

    public Task RollbackAsync(CancellationToken ct) => _context.Database.RollbackTransactionAsync(ct);
}