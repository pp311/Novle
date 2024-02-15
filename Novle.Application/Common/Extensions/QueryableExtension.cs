using System.Linq.Expressions;
using Novle.Application.Helpers;
using Novle.Domain.Entities.Base;

namespace Novle.Application.Common.Extensions;

public static class QueryableExtension
{
    public static IQueryable<TEntity> WhereIf<TEntity>(
        this IQueryable<TEntity> query, 
        bool condition, 
        Expression<Func<TEntity, bool>> predicate) where TEntity : Entity 
        => condition ? query.Where(predicate) : query;
    
    public static IQueryable<TEntity> OrderBy<TEntity>(
        this IQueryable<TEntity> query, 
        IOrderByField orderByField,
        bool isDescending) where TEntity : Entity
        => isDescending 
            ? Queryable.OrderByDescending(query, orderByField.GetExpression()) 
            : Queryable.OrderBy(query, orderByField.GetExpression());
}