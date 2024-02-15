using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Novle.Application.Common.Interfaces;
using Novle.Domain.Entities;
using Novle.Domain.Entities.Base;
using Novle.Infrastructure.Identity;

namespace Novle.Infrastructure.Repositories.Base;

public class NovleDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    private readonly ICurrentUser _currentUser;
    public NovleDbContext(DbContextOptions<NovleDbContext> options, ICurrentUser currentUser) : base(options)
    {
        _currentUser = currentUser;
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<BaseComment>().UseTpcMappingStrategy();
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Add soft delete query filter
        // https://www.thereformedprogrammer.net/ef-core-in-depth-soft-deleting-data-with-global-query-filters/
        var softDeleteEntityTypes = builder.Model
            .GetEntityTypes()
            .Where(e => e.ClrType.IsAssignableFrom(typeof(ISoftDelete)));
        
        foreach (var entityType in softDeleteEntityTypes)
            entityType.AddSoftDeleteQueryFilter();
        
        base.OnModelCreating(builder);
    }
    
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditableEntityProperties();
        SetSoftDeleteEntityProperties();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void SetAuditableEntityProperties()
    {
        var userId = int.TryParse(_currentUser.Id, out var id) ? id : 0;
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedBy = userId;
                    entry.Entity.UpdatedOn = DateTime.UtcNow;
                    break;
            }
        }
    }
    
    private void SetSoftDeleteEntityProperties()
    {
        foreach (var entry in ChangeTracker.Entries<ISoftDelete>())
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    break;
            }
        }
    }
}

public static class SoftDeleteQueryExtension
{
    public static void AddSoftDeleteQueryFilter(
        this IMutableEntityType entityData)
    {
        var methodToCall = typeof(SoftDeleteQueryExtension)
            .GetMethod(nameof(GetSoftDeleteFilter))!
            .MakeGenericMethod(entityData.ClrType);
        
        var filter = methodToCall.Invoke(null, []);
        
        entityData.SetQueryFilter((LambdaExpression)filter!);
        entityData.AddIndex(entityData.FindProperty(nameof(ISoftDelete.IsDeleted))!);
    }
 
    private static LambdaExpression GetSoftDeleteFilter<TEntity>() where TEntity : class, ISoftDelete
    {
        Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
        return filter;
    }
}
